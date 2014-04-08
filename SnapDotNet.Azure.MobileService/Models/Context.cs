using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using SnapDotNet.Azure.MobileService.DataObjects;

namespace SnapDotNet.Azure.MobileService.Models
{

	public class Context : DbContext
	{
		private const string ConnectionStringName = "Name=MS_TableConnectionString";

		public Context()
			: base(ConnectionStringName)
		{
		}

		public Context(string schema)
			: base(ConnectionStringName)
		{
			Schema = schema;
		}

		public string Schema { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Snap> Snaps { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (Schema != null)
			{
				modelBuilder.HasDefaultSchema(Schema);
			}

			modelBuilder.Conventions.Add(
				new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
					"ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
		}
	}

}
