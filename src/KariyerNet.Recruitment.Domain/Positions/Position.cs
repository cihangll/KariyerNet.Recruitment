using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.Positions;

public class Position : AuditedAggregateRoot<Guid>, IMultiTenant
{
	public virtual Guid? TenantId { get; set; }

	public virtual string Name { get; protected set; } = default!;

	public virtual string? Description { get; protected set; }

	protected Position() { }

	public Position(string name, string? description = null)
	{
		UpdateName(name);
		UpdateDescription(description);
	}

	public virtual Position UpdateName(string name)
	{
		Check.Length(name, nameof(name), PositionConsts.NameMaxLength);
		Name = name;

		return this;
	}

	public virtual Position UpdateDescription(string? description)
	{
		Check.Length(description, nameof(description), PositionConsts.DescriptionMaxLength);
		Description = description;

		return this;
	}
}