using KariyerNet.Recruitment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace KariyerNet.Recruitment.Permissions;

public class RecruitmentPermissionDefinitionProvider : PermissionDefinitionProvider
{
	public override void Define(IPermissionDefinitionContext context)
	{
		var appGroup = context.AddGroup(RecruitmentPermissions.GroupName);
		//Define your own permissions here. Example:
		//myGroup.AddPermission(RecruitmentPermissions.MyPermission1, L("Permission:MyPermission1"));

		var positionPermission = appGroup.AddPermission(
			RecruitmentPermissions.Positions.Default,
			L("Permissions:Positions"),
			MultiTenancySides.Tenant);
		positionPermission.AddChild(
			RecruitmentPermissions.Positions.Create,
			L("Permissions:Positions.Create"),
			MultiTenancySides.Tenant);
		positionPermission.AddChild(
			RecruitmentPermissions.Positions.Update,
			L("Permissions:Positions.Update"),
			MultiTenancySides.Tenant);
		positionPermission.AddChild(
			RecruitmentPermissions.Positions.Delete,
			L("Permissions:Positions.Delete"),
			MultiTenancySides.Tenant);

		var benefitPermission = appGroup.AddPermission(
			RecruitmentPermissions.Benefits.Default,
			L("Permissions:Benefits"),
			MultiTenancySides.Tenant);
		benefitPermission.AddChild(
			RecruitmentPermissions.Benefits.Create,
			L("Permissions:Benefits.Create"),
			MultiTenancySides.Tenant);
		benefitPermission.AddChild(
			RecruitmentPermissions.Benefits.Update,
			L("Permissions:Benefits.Update"),
			MultiTenancySides.Tenant);
		benefitPermission.AddChild(
			RecruitmentPermissions.Benefits.Delete,
			L("Permissions:Benefits.Delete"),
			MultiTenancySides.Tenant);

		var jobAdvertPermission = appGroup.AddPermission(
			RecruitmentPermissions.JobAdverts.Default,
			L("Permissions:JobAdverts"),
			MultiTenancySides.Tenant);
		jobAdvertPermission.AddChild(
			RecruitmentPermissions.JobAdverts.Create,
			L("Permissions:JobAdverts.Create"),
			MultiTenancySides.Tenant);
		jobAdvertPermission.AddChild(
			RecruitmentPermissions.JobAdverts.Update,
			L("Permissions:JobAdverts.Update"),
			MultiTenancySides.Tenant);
		jobAdvertPermission.AddChild(
			RecruitmentPermissions.JobAdverts.Delete,
			L("Permissions:JobAdverts.Delete"),
			MultiTenancySides.Tenant);

		var disabledWordPermission = appGroup.AddPermission(
			RecruitmentPermissions.DisabledWords.Default,
			L("Permissions:DisabledWords"),
			MultiTenancySides.Tenant);
		disabledWordPermission.AddChild(
			RecruitmentPermissions.DisabledWords.Create,
			L("Permissions:DisabledWords.Create"),
			MultiTenancySides.Tenant);
		disabledWordPermission.AddChild(
			RecruitmentPermissions.DisabledWords.Update,
			L("Permissions:DisabledWords.Update"),
			MultiTenancySides.Tenant);
		disabledWordPermission.AddChild(
			RecruitmentPermissions.DisabledWords.Delete,
			L("Permissions:DisabledWords.Delete"),
			MultiTenancySides.Tenant);
	}

	private static LocalizableString L(string name)
	{
		return LocalizableString.Create<RecruitmentResource>(name);
	}
}
