using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using SnapDotNet.Azure.MobileService.DataObjects;

namespace SnapDotNet.Azure.MobileService.Models
{

	public class Context : DbContext
	{
		// You can add custom code to this file. Changes will not be overwritten.
		// 
		// If you want Entity Framework to alter your database
		// automatically whenever you change your model schema, please use data migrations.
		// For more information refer to the documentation:
		// http://msdn.microsoft.com/en-us/data/jj591621.aspx
		//
		// To enable Entity Framework migrations in the cloud, please ensure that the 
		// service name, set by the 'MS_MobileServiceName' AppSettings in the local 
		// Web.config, is the same as the service name when hosted in Azure.

		private const string ConnectionStringName = "Name=MS_TableConnectionString";

		public Context() 
			: base(ConnectionStringName) { }

		public DbSet<User> Users { get; set; }
		public DbSet<SnapchatChat> SnapchatChats { get; set; }
		public DbSet<SnapchatSnap> SnapchatSnaps { get; set; }
		public DbSet<SnapchatAddedFriend> SnapchatAddedFriends { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var schema = ServiceSettingsDictionary.GetSchemaName();
			if (!string.IsNullOrEmpty(schema))
			{
				modelBuilder.HasDefaultSchema(schema);
			}

			// Set the Id column of every to be the PK
			modelBuilder.Properties<string>()
				.Where(p => p.Name == "Id")
				.Configure(p => p.IsKey());

			modelBuilder.Conventions.Add(
				new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
					"ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

			modelBuilder.Properties<DateTimeOffset?>()
				.Where(p => p.Name == "CreatedAt")
				.Configure(p => p.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));

			modelBuilder.Properties<DateTimeOffset?>()
				.Where(p => p.Name == "UpdatedAt")
				.Configure(p => p.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed));
		}
	}
}
