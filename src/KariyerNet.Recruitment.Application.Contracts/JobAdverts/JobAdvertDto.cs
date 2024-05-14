using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertDto : FullAuditedEntityDto<Guid>
{
	public Guid UserId { get; set; }

	public Guid PositionId { get; set; }

	public string Description { get; set; } = default!;

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public JobAdvertQuality? Quality { get; set; }

	public JobAdvertWorkType? WorkType { get; set; }

	public decimal? Salary { get; set; }

	public ICollection<JobAdvertBenefitDto>? JobAdvertBenefits { get; set; }
}