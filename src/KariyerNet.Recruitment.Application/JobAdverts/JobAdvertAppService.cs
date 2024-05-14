using KariyerNet.Recruitment.Benefits;
using KariyerNet.Recruitment.Features;
using KariyerNet.Recruitment.Permissions;
using KariyerNet.Recruitment.Positions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;


namespace KariyerNet.Recruitment.JobAdverts;

[Authorize(RecruitmentPermissions.JobAdverts.Default)]
public class JobAdvertAppService
	: CrudAppService<
		JobAdvert,
		JobAdvertDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateJobAdvertDto>
	, IJobAdvertAppService
{
	protected JobAdvertManager JobAdvertManager { get; }
	protected IRepository<Position, Guid> PositionRepository
		=> LazyServiceProvider.LazyGetRequiredService<IRepository<Position, Guid>>();

	protected IRepository<Benefit, Guid> BenefitRepository
		=> LazyServiceProvider.LazyGetRequiredService<IRepository<Benefit, Guid>>();

	protected IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();

	public JobAdvertAppService(
		IRepository<JobAdvert, Guid> repository,
		JobAdvertManager jobAdvertManager) : base(repository)
	{
		JobAdvertManager = jobAdvertManager;

		GetPolicyName = RecruitmentPermissions.JobAdverts.Default;
		GetListPolicyName = RecruitmentPermissions.JobAdverts.Default;
		CreatePolicyName = RecruitmentPermissions.JobAdverts.Create;
		UpdatePolicyName = RecruitmentPermissions.JobAdverts.Update;
		DeletePolicyName = RecruitmentPermissions.JobAdverts.Delete;
	}

	public async Task<JobAdvertDetailDto> GetWithDetailsAsync(Guid id)
	{
		Check.NotNull(CurrentUser, nameof(CurrentUser));

		var queryable = await Repository.WithDetailsAsync();

		var query = from jobAdvert in queryable
					join position in await PositionRepository.GetQueryableAsync() on jobAdvert.PositionId equals position.Id
					where jobAdvert.Id == id && jobAdvert.UserId == CurrentUser!.Id
					select new
					{
						JobAdvert = jobAdvert,
						PositionName = position.Name,
						PositionDescription = position.Description,
					};

		var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query) ?? throw new EntityNotFoundException(typeof(JobAdvert), id);

		var jobAdvertDetail = ObjectMapper.Map<JobAdvert, JobAdvertDetailDto>(queryResult!.JobAdvert);
		jobAdvertDetail.PositionName = queryResult!.PositionName;
		jobAdvertDetail.PositionDescription = queryResult!.PositionDescription;

		var jobAdvertBenefits = queryResult?.JobAdvert?.JobAdvertBenefits;
		if (jobAdvertBenefits is not null)
		{
			var benefits = (
				from jab in jobAdvertBenefits
				join benefit in await BenefitRepository.GetQueryableAsync() on jab.BenefitId equals benefit.Id
				select new SimpleBenefitDto
				{
					Id = benefit.Id,
					Name = benefit.Name,
					Description = benefit.Description
				}).ToList();

			jobAdvertDetail.Benefits = benefits;
		}

		return jobAdvertDetail;
	}

	public override async Task<JobAdvertDto> GetAsync(Guid id)
	{
		Check.NotNull(CurrentUser, nameof(CurrentUser));

		var jobAdvert = await Repository.GetAsync(x => x.Id == id && x.UserId == CurrentUser.Id);

		return ObjectMapper.Map<JobAdvert, JobAdvertDto>(jobAdvert);
	}

	public override async Task<PagedResultDto<JobAdvertDto>> GetListAsync(PagedAndSortedResultRequestDto input)
	{
		Check.NotNull(CurrentUser, nameof(CurrentUser));

		if (input.Sorting.IsNullOrWhiteSpace())
		{
			input.Sorting = nameof(JobAdvert.Description);
		}

		var query = (await Repository.WithDetailsAsync())
			.Where(x =>
				x.UserId == CurrentUser.Id
				// Should be active job advert
				&& Clock.Now < x.EndDate
			)
			.OrderBy(input.Sorting)
			.Skip(input.SkipCount)
			.Take(input.MaxResultCount);

		var jobAdverts = await AsyncExecuter.ToListAsync(query);

		var totalCount = await Repository.CountAsync(x => x.UserId == CurrentUser.Id);

		return new PagedResultDto<JobAdvertDto>(
			totalCount,
			ObjectMapper.Map<List<JobAdvert>, List<JobAdvertDto>>(jobAdverts)
		);
	}

	[Authorize(RecruitmentPermissions.JobAdverts.Create)]
	public override async Task<JobAdvertDto> CreateAsync(CreateUpdateJobAdvertDto input)
	{
		Check.NotNull(CurrentUser, nameof(CurrentUser));
		var currentUserId = CurrentUser.Id!.Value;

		var maxJobAdvertLimit = await FeatureChecker.GetAsync<int>(RecruitmentFeatures.MaxJobAdvertLimit, 2);
		var jobAdvertCount = await Repository.CountAsync(x => x.UserId == currentUserId);

		if (jobAdvertCount >= maxJobAdvertLimit)
		{
			throw new BusinessException(
				"Recruitment:ReachToMaxJobAdvertCountLimit",
				$"You can not create more than {maxJobAdvertLimit} job adverts!"
			);
		}

		var jobAdvert = await JobAdvertManager.CreateAsync(
			currentUserId,
			input.PositionId,
			input.Description,
			Clock.Now,
			Clock.Now.AddDays(14),
			null,
			input.WorkType,
			input.Salary,
			input.BenefitIds
		);

		await Repository.InsertAsync(jobAdvert);

		var eto = ObjectMapper.Map<JobAdvert, JobAdvertCreatedOrUpdatedEto>(jobAdvert);
		await DistributedEventBus.PublishAsync(eto);

		return ObjectMapper.Map<JobAdvert, JobAdvertDto>(jobAdvert);
	}

	[Authorize(RecruitmentPermissions.JobAdverts.Update)]
	public override async Task<JobAdvertDto> UpdateAsync(Guid id, CreateUpdateJobAdvertDto input)
	{
		Check.NotNull(CurrentUser, nameof(CurrentUser));

		var jobAdvert = await Repository.GetAsync(id);

		if (jobAdvert.UserId != CurrentUser!.Id)
		{
			throw new InvalidOperationException("Invalid user");
		}

		jobAdvert = await JobAdvertManager.UpdatePositionAsync(jobAdvert, input.PositionId);

		jobAdvert.UpdateDescription(input.Description);

		// It will be recalculated by RabbitMQ service
		jobAdvert.ResetQuality();

		jobAdvert.Salary = input.Salary;
		jobAdvert.WorkType = input.WorkType;

		var validBenefitIds = await JobAdvertManager.GetValidBenefitIdsAsync(input.BenefitIds);
		var existBenefitIds = jobAdvert.JobAdvertBenefits?.Select(x => x.BenefitId).ToList() ?? [];

		var wantToAddBenefitIds = validBenefitIds.Except(existBenefitIds).ToList();
		foreach (var wantToAddBenefitId in wantToAddBenefitIds)
		{
			jobAdvert.AddJobAdvertBenefit(GuidGenerator.Create(), wantToAddBenefitId);
		}

		var wantToDeleteBenefitIds = existBenefitIds.Except(validBenefitIds).ToList();
		foreach (var wantToDeleteBenefitId in wantToDeleteBenefitIds)
		{
			jobAdvert.RemoveJobAdvertBenefit(wantToDeleteBenefitId);
		}

		await Repository.UpdateAsync(jobAdvert);

		var eto = ObjectMapper.Map<JobAdvert, JobAdvertCreatedOrUpdatedEto>(jobAdvert);
		await DistributedEventBus.PublishAsync(eto);

		return ObjectMapper.Map<JobAdvert, JobAdvertDto>(jobAdvert);
	}
}
