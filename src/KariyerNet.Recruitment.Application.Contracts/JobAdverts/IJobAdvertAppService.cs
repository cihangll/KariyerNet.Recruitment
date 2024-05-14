using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace KariyerNet.Recruitment.JobAdverts;

public interface IJobAdvertAppService
	: ICrudAppService<
		JobAdvertDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateJobAdvertDto>
{
	Task<JobAdvertDetailDto> GetWithDetailsAsync(Guid id);
}
