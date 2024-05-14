using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace KariyerNet.Recruitment.Positions;

public interface IPositionAppService : ICrudAppService<
		PositionDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdatePositionDto
	>
{
}
