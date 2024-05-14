using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.Benefits;

public class Benefit : AuditedAggregateRoot<Guid>, IMultiTenant
{
	public virtual Guid? TenantId { get; set; }

	public virtual string Name { get; protected set; } = default!;

	public virtual string? Description { get; protected set; }

	protected Benefit() { }

	public Benefit(string name, string? description = null)
	{
		UpdateName(name);
		UpdateDescription(description);
	}

	public virtual Benefit UpdateName(string name)
	{
		Check.Length(name, nameof(name), BenefitConsts.NameMaxLength);
		Name = name;

		return this;
	}

	public virtual Benefit UpdateDescription(string? description)
	{
		Check.Length(description, nameof(description), BenefitConsts.DescriptionMaxLength);
		Description = description;

		return this;
	}
}
