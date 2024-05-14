using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertBenefit : AuditedEntity<Guid>, IMultiTenant
{
	public virtual Guid? TenantId { get; set; }

	public virtual Guid JobAdvertId { get; protected set; }

	public virtual Guid BenefitId { get; protected set; }

	protected JobAdvertBenefit() { }

	internal JobAdvertBenefit(Guid id, Guid jobAdvertId, Guid benefitId) : base(id)
	{
		Update(jobAdvertId, benefitId);
	}

	internal virtual JobAdvertBenefit Update(Guid jobAdvertId, Guid benefitId)
	{
		JobAdvertId = jobAdvertId;
		BenefitId = benefitId;

		return this;
	}
}
