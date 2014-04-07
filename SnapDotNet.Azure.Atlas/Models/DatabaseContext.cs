using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using SnapDotNet.Azure.Atlas.DataObjects;

namespace SnapDotNet.Azure.Atlas.Models
{

	public class DatabaseContext : DbContext
	{
		public DatabaseContext() 
			: base("MS_TableConnectionString") { }

		public string Schema { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Snap> Snaps { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (Schema != null)
				modelBuilder.HasDefaultSchema(Schema);

			modelBuilder.Conventions.Add(
				new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
					"ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
		}
	}
}
