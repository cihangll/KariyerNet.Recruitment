using System;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.DisabledWords;

public class DisabledWordDto : AuditedEntityDto<Guid>
{
	public string Name { get; set; } = default!;
}
