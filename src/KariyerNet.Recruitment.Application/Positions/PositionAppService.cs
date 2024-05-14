using KariyerNet.Recruitment.JobAdverts;
using KariyerNet.Recruitment.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace KariyerNet.Recruitment.Positions;

[Authorize(RecruitmentPermissions.Positions.Default)]
public class PositionAppService
	: CrudAppService<
		Position,
		PositionDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdatePositionDto>,
	IPositionAppService
{
	protected IRepository<JobAdvert, Guid> JobAdvertRepository => LazyServiceProvider.GetRequiredService<IRepository<JobAdvert, Guid>>();

	public PositionAppService(IRepository<Position, Guid> repository) : base(repository)
	{
		GetPolicyName = RecruitmentPermissions.Positions.Default;
		GetListPolicyName = RecruitmentPermissions.Positions.Default;
		CreatePolicyName = RecruitmentPermissions.Positions.Create;
		UpdatePolicyName = RecruitmentPermissions.Positions.Update;
		DeletePolicyName = RecruitmentPermissions.Positions.Delete;
	}

	[Authorize(RecruitmentPermissions.Positions.Create)]
	public override async Task<PositionDto> CreateAsync(CreateUpdatePositionDto input)
	{
		if (await Repository.AnyAsync(x => x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.CreateAsync(input);
	}

	[Authorize(RecruitmentPermissions.Positions.Update)]
	public override async Task<PositionDto> UpdateAsync(Guid id, CreateUpdatePositionDto input)
	{
		if (await Repository.AnyAsync(x => x.Id != id && x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.UpdateAsync(id, input);
	}

	[Authorize(RecruitmentPermissions.Positions.Delete)]
	protected override async Task DeleteByIdAsync(Guid id)
	{
		var query = from jobAdvert in await JobAdvertRepository.GetQueryableAsync()
					join position in await Repository.GetQueryableAsync() on jobAdvert.PositionId equals position.Id
					where position.Id == id
					select position.Id;

		if (await AsyncExecuter.AnyAsync(query))
		{
			throw new UserFriendlyException(L["CannotDeleteJobAdvertRelationExist"]);
		}

		await base.DeleteByIdAsync(id);
	}
}
