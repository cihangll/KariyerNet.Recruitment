using System;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.Benefits;

public class BenefitDto : AuditedEntityDto<Guid>
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
}
