using KariyerNet.Recruitment.JobAdverts;
using KariyerNet.Recruitment.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace KariyerNet.Recruitment.Benefits;

[Authorize(RecruitmentPermissions.Benefits.Default)]
public class BenefitAppService
	: CrudAppService<
		Benefit,
		BenefitDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateBenefitDto>,
	IBenefitAppService
{
	protected IRepository<JobAdvertBenefit, Guid> JobAdvertBenefitRepository => LazyServiceProvider.GetRequiredService<IRepository<JobAdvertBenefit, Guid>>();

	public BenefitAppService(
		IRepository<Benefit, Guid> repository) : base(repository)
	{
		GetPolicyName = RecruitmentPermissions.Benefits.Default;
		GetListPolicyName = RecruitmentPermissions.Benefits.Default;
		CreatePolicyName = RecruitmentPermissions.Benefits.Create;
		UpdatePolicyName = RecruitmentPermissions.Benefits.Update;
		DeletePolicyName = RecruitmentPermissions.Benefits.Delete;
	}

	[Authorize(RecruitmentPermissions.Benefits.Create)]
	public override async Task<BenefitDto> CreateAsync(CreateUpdateBenefitDto input)
	{
		if (await Repository.AnyAsync(x => x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.CreateAsync(input);
	}

	[Authorize(RecruitmentPermissions.Benefits.Update)]
	public override async Task<BenefitDto> UpdateAsync(Guid id, CreateUpdateBenefitDto input)
	{
		if (await Repository.AnyAsync(x => x.Id != id && x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.UpdateAsync(id, input);
	}

	[Authorize(RecruitmentPermissions.Benefits.Delete)]
	protected override async Task DeleteByIdAsync(Guid id)
	{
		var query = from jobAdvertBenefit in await JobAdvertBenefitRepository.GetQueryableAsync()
					join benefit in await Repository.GetQueryableAsync() on jobAdvertBenefit.BenefitId equals benefit.Id
					where benefit.Id == id
					select benefit.Id;

		if (await AsyncExecuter.AnyAsync(query))
		{
			throw new UserFriendlyException(L["CannotDeleteJobAdvertBenefitRelationExist"]);
		}

		await base.DeleteByIdAsync(id);
	}
}
