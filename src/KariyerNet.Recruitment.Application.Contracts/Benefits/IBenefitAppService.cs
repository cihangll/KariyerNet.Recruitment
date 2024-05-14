using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace KariyerNet.Recruitment.Benefits;

public interface IBenefitAppService : ICrudAppService<
		BenefitDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateBenefitDto
	>
{
}