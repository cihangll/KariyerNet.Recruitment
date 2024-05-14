using System;
using Volo.Abp.Application.Dtos;

namespace KariyerNet.Recruitment.JobAdverts;

public class JobAdvertBenefitDto : AuditedEntityDto<Guid>
{
	public Guid JobAdvertId { get; set; }
	public Guid BenefitId { get; set; }
}
