using KariyerNet.Recruitment.Benefits;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertDetailDto : FullAuditedEntityDto<Guid>
{
	public Guid UserId { get; set; }

	public Guid PositionId { get; set; }

	public string PositionName { get; set; } = default!;

	public string? PositionDescription { get; set; }

	public string Description { get; set; } = default!;

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public JobAdvertQuality? Quality { get; set; }

	public JobAdvertWorkType? WorkType { get; set; }

	public decimal? Salary { get; set; }

	public ICollection<SimpleBenefitDto> Benefits { get; set; } = new List<SimpleBenefitDto>();
}