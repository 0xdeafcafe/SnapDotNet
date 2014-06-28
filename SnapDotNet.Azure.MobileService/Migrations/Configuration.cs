using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Context>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
		}

		protected override void Seed(Context context)
		{
			//context.Users.AddOrUpdate(new[]
			//{
			//	new User 
			//	{ 
			//		SnapchatUsername = "swagyo", 
			//		Subscribed = false, 
			//		Probation = true, 
			//		AuthTokenExpired = true,
			//		SnapNotify = true,
			//		ChatNotify = true,
			//		DeviceId = "",
			//		LastPushServed = DateTime.UtcNow,
			//		NewUser = true,
			//		NextUpdate = DateTime.UtcNow,
			//		ScreenshotNotify = true,
			//		AuthToken = ""
			//	}
			//});
		}

		internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
		{
			protected override void Generate(AddColumnOperation addColumnOperation)
			{
				SetCreatedUtcColumn(addColumnOperation.Column);

				base.Generate(addColumnOperation);
			}

			protected override void Generate(CreateTableOperation createTableOperation)
			{
				SetCreatedUtcColumn(createTableOperation.Columns);

				base.Generate(createTableOperation);
			}

			private static void SetCreatedUtcColumn(IEnumerable<ColumnModel> columns)
			{
				foreach (var columnModel in columns)
				{
					SetCreatedUtcColumn(columnModel);
				}
			}

			private static void SetCreatedUtcColumn(PropertyModel column)
			{
				if (column.Name == "CreatedAt")
				{
					column.DefaultValueSql = "GETUTCDATE()";
				}
			}
		}
	}
}
