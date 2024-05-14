using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvert : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
	/// <summary>
	/// Tenant Id
	/// </summary>
	public virtual Guid? TenantId { get; set; }

	/// <summary>
	/// İlan Sahibi Kullanıcı Id
	/// </summary>
	public virtual Guid UserId { get; protected set; }

	/// <summary>
	/// Pozisyon Id
	/// </summary>
	public virtual Guid PositionId { get; protected set; }

	/// <summary>
	/// İlan Açıklaması
	/// </summary>
	public virtual string Description { get; protected set; } = default!;

	/// <summary>
	/// İlan Başlangıç Tarihi
	/// </summary>
	public virtual DateTime StartDate { get; protected set; }

	/// <summary>
	/// İlan Bitiş Tarihi
	/// </summary>
	public virtual DateTime EndDate { get; protected set; }

	/// <summary>
	/// İlan Kalitesi
	/// </summary>
	public virtual JobAdvertQuality? Quality { get; protected set; }

	/// <summary>
	/// Çalışma Türü
	/// </summary>
	public virtual JobAdvertWorkType? WorkType { get; set; }

	/// <summary>
	/// Ücret Bilgisi
	/// </summary>
	public virtual decimal? Salary { get; set; }

	/// <summary>
	/// Yan Haklar
	/// </summary>
	public ICollection<JobAdvertBenefit>? JobAdvertBenefits { get; protected set; }

	protected JobAdvert() { }

	internal JobAdvert(
		Guid id,
		Guid userId,
		Guid positionId,
		string description,
		DateTime startDate,
		DateTime endDate,
		JobAdvertQuality? quality = null,
		JobAdvertWorkType? workType = null,
		decimal? salary = null) : base(id)
	{
		UserId = userId;

		UpdatePosition(positionId);
		UpdateDescription(description);
		SetDateInterval(startDate, endDate);

		Quality = quality;
		WorkType = workType;
		Salary = salary;
	}

	internal virtual JobAdvert UpdatePosition(Guid positionId)
	{
		PositionId = positionId;

		return this;
	}

	public virtual JobAdvert UpdateDescription(string description)
	{
		Check.NotNullOrWhiteSpace(description, nameof(Description), JobAdvertConsts.DescriptionMaxLength);
		Description = description;

		return this;
	}

	public virtual JobAdvert UpdateQuality(JobAdvertQuality quality)
	{
		Quality = quality;
		return this;
	}

	public virtual JobAdvert ResetQuality()
	{
		Quality = null;
		return this;
	}

	internal virtual JobAdvert SetDateInterval(DateTime startDate, DateTime endDate)
	{
		if (startDate > endDate)
		{
			throw new BusinessException(RecruitmentDomainErrorCodes.EndDateCantBeEarlierThanStartDate);
		}

		StartDate = startDate;
		EndDate = endDate;
		return this;
	}

	public virtual ICollection<JobAdvertBenefit> AddJobAdvertBenefit(Guid id, Guid benefitId)
	{
		JobAdvertBenefits ??= new Collection<JobAdvertBenefit>();

		JobAdvertBenefits.Add(new JobAdvertBenefit(id: id, jobAdvertId: Id, benefitId: benefitId));

		return JobAdvertBenefits;
	}

	public virtual ICollection<JobAdvertBenefit> RemoveJobAdvertBenefit(Guid benefitId)
	{
		JobAdvertBenefits ??= new Collection<JobAdvertBenefit>();

		var jobAdvertBenefit = JobAdvertBenefits.FirstOrDefault(x => x.BenefitId == benefitId);
		if (jobAdvertBenefit is null)
		{
			return JobAdvertBenefits;
		}

		JobAdvertBenefits.Remove(jobAdvertBenefit);

		return JobAdvertBenefits;
	}
}
