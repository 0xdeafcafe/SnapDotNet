﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using SnapDotNet.Azure.Atlas.DataObjects;

namespace SnapDotNet.Azure.Atlas.Models
{

	public class Context : DbContext
	{
		// You can add custom code to this file. Changes will not be overwritten.
		// 
		// If you want Entity Framework to alter your database
		// automatically whenever you change your model schema, please use data migrations.
		// For more information refer to the documentation:
		// http://msdn.microsoft.com/en-us/data/jj591621.aspx

		private const string ConnectionStringName = "Name=MS_TableConnectionString";

		public Context()
			: base(ConnectionStringName)
		{
		}

		// When using code first migrations, ensure you use this constructor
		// and you specify a schema, which is the same as your mobile service name.
		// You can do that by registering an instance of IDbContextFactory<T>.
		public Context(string schema)
			: base(ConnectionStringName)
		{
			Schema = schema;
		}

		public string Schema { get; set; }

		public DbSet<TodoItem> TodoItems { get; set; }

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
