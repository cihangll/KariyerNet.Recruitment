namespace KariyerNet.Recruitment.Permissions;

public static class RecruitmentPermissions
{
	public const string GroupName = "Recruitment";

	//Add your own permission names. Example:
	//public const string MyPermission1 = GroupName + ".MyPermission1";

	public static class Positions
	{
		public const string Default = GroupName + ".Positions";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}

	public static class Benefits
	{
		public const string Default = GroupName + ".Benefits";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}

	public static class JobAdverts
	{
		public const string Default = GroupName + ".JobAdverts";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}

	public static class DisabledWords
	{
		public const string Default = GroupName + ".DisabledWords";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}
}
