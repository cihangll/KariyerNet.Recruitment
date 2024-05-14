using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace KariyerNet.Recruitment.DisabledWords;

public interface IDisabledWordAppService : ICrudAppService<
		DisabledWordDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateDisabledWordDto
	>
{
}
