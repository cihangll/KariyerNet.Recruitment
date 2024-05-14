using System;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.Positions;

public class PositionDto : AuditedEntityDto<Guid>
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
}
