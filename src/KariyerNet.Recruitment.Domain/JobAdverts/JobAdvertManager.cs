using KariyerNet.Recruitment.Benefits;
using KariyerNet.Recruitment.Positions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertManager : DomainService
{
	protected IRepository<Position, Guid> PositionRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<Position, Guid>>();

	protected IRepository<Benefit, Guid> BenefitRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<Benefit, Guid>>();

	public virtual async Task<JobAdvert> CreateAsync(
		Guid userId,
		Guid positionId,
		string description,
		DateTime startDate,
		DateTime endDate,
		JobAdvertQuality? quality = null,
		JobAdvertWorkType? workType = null,
		decimal? salary = null,
		ICollection<Guid>? benefitIds = null)
	{
		await PositionRepository.EnsureExistsAsync(positionId);

		benefitIds = await GetValidBenefitIdsAsync(benefitIds);

		var entity = new JobAdvert(
			GuidGenerator.Create(),
			userId,
			positionId,
			description,
			startDate,
			endDate,
			quality,
			workType,
			salary);

		foreach (var benefitId in benefitIds)
		{
			entity.AddJobAdvertBenefit(GuidGenerator.Create(), benefitId);
		}

		return entity;
	}

	public virtual async Task<JobAdvert> UpdatePositionAsync(JobAdvert entity, Guid positionId)
	{
		if (entity.PositionId == positionId) return entity;

		await PositionRepository.EnsureExistsAsync(positionId);

		entity.UpdatePosition(positionId);

		return entity;
	}

	public virtual async Task<ICollection<Guid>> GetValidBenefitIdsAsync(ICollection<Guid>? benefitIds)
	{
		if (benefitIds is null) return new Collection<Guid>();

		var queryableBenefits = await BenefitRepository.GetQueryableAsync();

		var existBenefitIdsAsQueryable = queryableBenefits
			.Where(x => benefitIds.Contains(x.Id))
			.Select(x => x.Id);

		benefitIds = await AsyncExecuter.ToListAsync(existBenefitIdsAsQueryable);

		return benefitIds ?? new Collection<Guid>();
	}
}
