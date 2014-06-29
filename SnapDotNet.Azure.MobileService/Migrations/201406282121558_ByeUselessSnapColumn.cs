namespace SnapDotNet.Azure.MobileService.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class ByeUselessSnapColumn : DbMigration
	{
		public override void Up()
		{
			DropColumn("snapdotnet.SnapchatSnaps", "SnapMediaId");
		}

		public override void Down()
		{
			AddColumn("snapdotnet.SnapchatSnaps", "SnapMediaId", c => c.String());
		}
	}
}
