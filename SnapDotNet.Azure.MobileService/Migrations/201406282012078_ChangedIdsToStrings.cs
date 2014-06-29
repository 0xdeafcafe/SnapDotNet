namespace SnapDotNet.Azure.MobileService.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class ChangedIdsToStrings : DbMigration
	{
		public override void Up()
		{
			AlterColumn("snapdotnet.SnapchatAddedFriends", "UserId", c => c.String());
			AlterColumn("snapdotnet.SnapchatChats", "UserId", c => c.String());
			AlterColumn("snapdotnet.SnapchatSnaps", "UserId", c => c.String());
		}

		public override void Down()
		{
			AlterColumn("snapdotnet.SnapchatSnaps", "UserId", c => c.Long(nullable: false));
			AlterColumn("snapdotnet.SnapchatChats", "UserId", c => c.Long(nullable: false));
			AlterColumn("snapdotnet.SnapchatAddedFriends", "UserId", c => c.Long(nullable: false));
		}
	}
}
