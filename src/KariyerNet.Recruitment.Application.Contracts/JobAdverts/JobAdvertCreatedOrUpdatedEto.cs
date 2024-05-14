using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.JobAdverts;

[EventName("JobAdvert.CreatedOrUpdated")]
public class JobAdvertCreatedOrUpdatedEto : IMultiTenant
{
	public Guid? TenantId { get; set; }
	public Guid Id { get; set; }
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
