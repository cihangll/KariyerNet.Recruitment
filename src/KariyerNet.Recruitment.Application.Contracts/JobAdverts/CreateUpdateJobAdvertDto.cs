using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KariyerNet.Recruitment.JobAdverts;

public class CreateUpdateJobAdvertDto
{
	public Guid PositionId { get; set; }

	public string Description { get; set; } = default!;

	public JobAdvertWorkType? WorkType { get; set; }

	public decimal? Salary { get; set; }

	public ICollection<Guid> BenefitIds { get; set; } = new Collection<Guid>();
}