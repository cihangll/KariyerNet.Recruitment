using KariyerNet.Recruitment.Benefits;
using KariyerNet.Recruitment.DisabledWords;
using KariyerNet.Recruitment.JobAdverts;
using KariyerNet.Recruitment.Positions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace KariyerNet.Recruitment.EntityFrameworkCore;

public static class RecruitmentDbContextModelCreatingExtensions
{
	public static void ConfigurePositions(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));

		builder.Entity<Position>(b =>
		{
			// Configs
			b.ToTable(RecruitmentConsts.DbTablePrefix + "Positions", RecruitmentConsts.DbSchema);

			b
				.Property(x => x.Name)
				.HasMaxLength(PositionConsts.NameMaxLength);

			b
				.Property(x => x.Description)
				.HasMaxLength(PositionConsts.DescriptionMaxLength);


			// Indexes
			b
				.HasIndex(x => new { x.Name, x.TenantId })
				.IsUnique(true);

			// Relations

			// Final Section
			b.ConfigureByConvention(); //auto configure for the base class props
		});
	}

	public static void ConfigureBenefits(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));

		builder.Entity<Benefit>(b =>
		{
			// Configs
			b.ToTable(RecruitmentConsts.DbTablePrefix + "Benefits", RecruitmentConsts.DbSchema);

			b
				.Property(x => x.Name)
				.HasMaxLength(BenefitConsts.NameMaxLength);

			b
				.Property(x => x.Description)
				.HasMaxLength(BenefitConsts.DescriptionMaxLength);

			// Indexes
			b
				.HasIndex(x => new { x.Name, x.TenantId })
				.IsUnique(true);

			// Relations

			// Final Section
			b.ConfigureByConvention(); //auto configure for the base class props
		});
	}

	public static void ConfigureJobAdvertBenefits(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));

		builder.Entity<JobAdvertBenefit>(b =>
		{
			// Configs
			b.ToTable(RecruitmentConsts.DbTablePrefix + "JobAdvertBenefits", RecruitmentConsts.DbSchema);

			// Indexes

			// Relations
			b
				.HasOne<Benefit>()
				.WithMany()
				.HasForeignKey(x => x.BenefitId)
				.OnDelete(DeleteBehavior.Restrict);

			// Final Section
			b.ConfigureByConvention(); //auto configure for the base class props
		});
	}

	public static void ConfigureJobAdverts(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));

		builder.Entity<JobAdvert>(b =>
		{
			// Configs
			b.ToTable(RecruitmentConsts.DbTablePrefix + "JobAdverts", RecruitmentConsts.DbSchema);

			b
				.Property(x => x.Description)
				.HasMaxLength(JobAdvertConsts.DescriptionMaxLength);

			b
				.Property(x => x.Salary)
				.IsRequired(false)
				.HasPrecision(18, 2);

			// Indexes

			// Relations
			b
				.HasOne<IdentityUser>()
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			b
				.HasOne<Position>()
				.WithMany()
				.HasForeignKey(x => x.PositionId)
				.OnDelete(DeleteBehavior.Restrict);

			// Final Section
			b.ConfigureByConvention(); //auto configure for the base class props
		});
	}

	public static void ConfigureDisabledWords(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));

		builder.Entity<DisabledWord>(b =>
		{
			// Configs
			b.ToTable(RecruitmentConsts.DbTablePrefix + "DisabledWords", RecruitmentConsts.DbSchema);

			b
				.Property(x => x.Name)
				.HasMaxLength(BenefitConsts.NameMaxLength);

			// Indexes
			b
				.HasIndex(x => new { x.Name, x.TenantId })
				.IsUnique(true);

			// Relations

			// Final Section
			b.ConfigureByConvention(); //auto configure for the base class props
		});
	}
}
