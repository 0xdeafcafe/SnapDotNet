namespace SnapDotNet.Azure.MobileService.Migrations
{
	using System.Collections.Generic;
	using System.Data.Entity.Infrastructure.Annotations;
	using System.Data.Entity.Migrations;

	public partial class AddedUser : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"snapdotnet.Users",
				c => new
					{
						Id = c.String(nullable: false, maxLength: 128,
							annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
						DeviceId = c.String(),
						SnapchatUsername = c.String(),
						Subscribed = c.Boolean(nullable: false),
						SnapNotify = c.Boolean(nullable: false),
						ChatNotify = c.Boolean(nullable: false),
						ScreenshotNotify = c.Boolean(nullable: false),
						NextUpdate = c.DateTime(nullable: false),
						Probation = c.Boolean(nullable: false),
						LastPushServed = c.DateTime(nullable: false),
						NewUser = c.Boolean(nullable: false),
						AuthToken = c.String(),
						AuthTokenExpired = c.Boolean(nullable: false),
						CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
							annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
						Deleted = c.Boolean(nullable: false,
							annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
						UpdatedAt = c.DateTimeOffset(precision: 7,
							annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
						Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
							annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
					})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropTable("snapdotnet.Users",
				removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
		}
	}
}
