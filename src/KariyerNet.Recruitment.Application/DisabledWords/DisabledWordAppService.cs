using KariyerNet.Recruitment.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace KariyerNet.Recruitment.DisabledWords;

[Authorize(RecruitmentPermissions.DisabledWords.Default)]
public class DisabledWordAppService
	: CrudAppService<
		DisabledWord,
		DisabledWordDto,
		Guid,
		PagedAndSortedResultRequestDto,
		CreateUpdateDisabledWordDto>,
	IDisabledWordAppService
{
	public DisabledWordAppService(IRepository<DisabledWord, Guid> repository) : base(repository)
	{
		GetPolicyName = RecruitmentPermissions.DisabledWords.Default;
		GetListPolicyName = RecruitmentPermissions.DisabledWords.Default;
		CreatePolicyName = RecruitmentPermissions.DisabledWords.Create;
		UpdatePolicyName = RecruitmentPermissions.DisabledWords.Update;
		DeletePolicyName = RecruitmentPermissions.DisabledWords.Delete;
	}

	[Authorize(RecruitmentPermissions.DisabledWords.Create)]
	public override async Task<DisabledWordDto> CreateAsync(CreateUpdateDisabledWordDto input)
	{
		if (await Repository.AnyAsync(x => x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.CreateAsync(input);
	}

	[Authorize(RecruitmentPermissions.DisabledWords.Update)]
	public override async Task<DisabledWordDto> UpdateAsync(Guid id, CreateUpdateDisabledWordDto input)
	{
		if (await Repository.AnyAsync(x => x.Id != id && x.Name == input.Name))
		{
			throw new UserFriendlyException(L["DuplicateNameError", input.Name]);
		}

		return await base.UpdateAsync(id, input);
	}

	[Authorize(RecruitmentPermissions.DisabledWords.Delete)]
	protected override async Task DeleteByIdAsync(Guid id)
	{
		//var anyMappingExist = await ReadOnlyWarehouseMappingRepository.AnyAsync(x => x.LogoWarehouseId == id);
		var anyMappingExist = false;
		if (anyMappingExist)
		{
			throw new UserFriendlyException(L["CannotDeleteRelationExist"]);
		}

		await base.DeleteByIdAsync(id);
	}
}
