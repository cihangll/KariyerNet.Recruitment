using KariyerNet.Recruitment.DisabledWords;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertCreatedOrUpdatedEventHandler
	  : IDistributedEventHandler<JobAdvertCreatedOrUpdatedEto>,
		ITransientDependency
{
	protected IRepository<JobAdvert, Guid> JobAdvertRepository { get; }

	protected IReadOnlyRepository<DisabledWord, Guid> DisabledWordRepository { get; }

	protected IUnitOfWorkManager UnitOfWorkManager { get; }

	protected ILogger Logger;

	public JobAdvertCreatedOrUpdatedEventHandler(
		ILogger<JobAdvertCreatedOrUpdatedEventHandler> logger,
		IUnitOfWorkManager unitOfWorkManager,
		IRepository<JobAdvert, Guid> jobAdvertRepository,
		IReadOnlyRepository<DisabledWord, Guid> disabledWordRepository)
	{
		Logger = logger;
		UnitOfWorkManager = unitOfWorkManager;
		JobAdvertRepository = jobAdvertRepository;
		DisabledWordRepository = disabledWordRepository;
	}

	public async Task HandleEventAsync(JobAdvertCreatedOrUpdatedEto eventData)
	{
		using var uow = UnitOfWorkManager.Begin();
		try
		{
			var jobAdvertEntity = await JobAdvertRepository.GetAsync(eventData.Id);

			var quality = await GetQualityAsync(eventData, DisabledWordRepository);
			jobAdvertEntity.UpdateQuality(quality);

			await JobAdvertRepository.UpdateAsync(jobAdvertEntity);

			await uow.SaveChangesAsync();
			await uow.CompleteAsync();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, ex.Message);
			await uow.RollbackAsync();
		}
	}

	private static async Task<JobAdvertQuality> GetQualityAsync(JobAdvertCreatedOrUpdatedEto eventData, IReadOnlyRepository<DisabledWord, Guid> disabledWordRepository)
	{
		byte score = 0;

		if (eventData.WorkType.HasValue) score += 1;

		if (eventData.Salary.HasValue) score += 1;

		if (eventData.JobAdvertBenefits?.Count > 0) score += 1;

		var upperDescriptionValue = eventData.Description.ToUpperInvariant();
		if (await disabledWordRepository.AllAsync(x => !upperDescriptionValue.Contains(x.NormalizedName)))
		{
			score += 2;
		}

		return (JobAdvertQuality)score;
	}
}