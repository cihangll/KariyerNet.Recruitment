using KariyerNet.Recruitment.Benefits;
using KariyerNet.Recruitment.DisabledWords;
using KariyerNet.Recruitment.Positions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.Data;

public class RecruitmentDataSeederContributor : IDataSeedContributor, ITransientDependency
{
	protected ICurrentTenant CurrentTenant { get; }
	protected IRepository<Position, Guid> PositionRepository { get; }
	protected IRepository<Benefit, Guid> BenefitRepository { get; }
	protected IRepository<DisabledWord, Guid> DisabledWordRepository { get; }

	public RecruitmentDataSeederContributor(
		ICurrentTenant currentTenant,
		IRepository<Position, Guid> positionRepository,
		IRepository<Benefit, Guid> benefitRepository,
		IRepository<DisabledWord, Guid> disabledWordRepository)
	{
		CurrentTenant = currentTenant;
		PositionRepository = positionRepository;
		BenefitRepository = benefitRepository;
		DisabledWordRepository = disabledWordRepository;
	}

	public async Task SeedAsync(DataSeedContext context)
	{
		await SeedPositionsAsync(context);
		await SeedBenefitsAsync(context);
		await SeedDisabledWordsAsync(context);
	}

	protected virtual async Task SeedPositionsAsync(DataSeedContext context)
	{
		using (CurrentTenant.Change(context?.TenantId))
		{
			// Seed only host
			if (context?.TenantId is null) return;

			if (await PositionRepository.GetCountAsync() > 0) return;

			var positions = new List<Position>{
				new ("Yazılım Uzmanı", "Yazılım Uzmanı"),
				new ("Muhasebe Elemanı"),
				new ("Diş Hekimi", ""),
				new ("Full Stack Developer", "c# dotnet core"),
				new ("React Developer", ""),
				new ("Backend Developer", ""),
			};

			await PositionRepository.InsertManyAsync(positions);
		}
	}

	protected virtual async Task SeedBenefitsAsync(DataSeedContext context)
	{
		using (CurrentTenant.Change(context?.TenantId))
		{
			// Seed only host
			if (context?.TenantId is null) return;

			if (await BenefitRepository.GetCountAsync() > 0) return;

			var benefits = new List<Benefit>{
				new ("Yol ücreti", "Günlük yol ücreti verilir."),
				new ("Yemek ücreti", "Günlük yemek ücreti verilir."),
				new ("Özel Sağlık Sigortası"),
				new ("Prim"),
				new ("Mesai Ücreti"),
			};

			await BenefitRepository.InsertManyAsync(benefits);
		}
	}

	protected virtual async Task SeedDisabledWordsAsync(DataSeedContext context)
	{
		using (CurrentTenant.Change(context?.TenantId))
		{
			// Seed only host
			if (context?.TenantId is null) return;

			if (await DisabledWordRepository.GetCountAsync() > 0) return;

			var disabledWords = new List<DisabledWord>{
				new ("diSABled Text 1"),
				new ("DisabLED TEXt 2"),
				new ("Disabled teXT 3"),
				new ("Disabled TexT 4"),
				new ("Disabled TEXT 5"),
				// Türkçe karakter örneği
				new ("Ölüm"),
				new("Çağrı")
			};

			await DisabledWordRepository.InsertManyAsync(disabledWords);
		}
	}
}
