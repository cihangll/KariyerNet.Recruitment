using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.DisabledWords;

public class DisabledWord : FullAuditedEntity<Guid>, IMultiTenant
{
	public virtual Guid? TenantId { get; set; }

	public virtual string Name { get; protected set; } = default!;

	public virtual string NormalizedName { get; protected set; } = default!;

	protected DisabledWord() { }

	public DisabledWord(string name)
	{
		UpdateName(name);
	}

	public DisabledWord UpdateName(string name)
	{
		Check.NotNullOrWhiteSpace(name, nameof(name), DisabledWordConsts.NameMaxLength);

		Name = name;
		NormalizedName = name.ToUpperInvariant();

		return this;
	}
}
