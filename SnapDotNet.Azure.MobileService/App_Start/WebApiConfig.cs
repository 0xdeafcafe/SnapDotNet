﻿using System.Data.Entity;
using Microsoft.WindowsAzure.Mobile.Service;
using SnapDotNet.Azure.MobileService.Models;

namespace SnapDotNet.Azure.MobileService
{
	public static class WebApiConfig
	{
		public static void Register()
		{
			// Use this class to set configuration options for your mobile service
			var options = new ConfigOptions();

			// Use this class to set WebAPI configuration options
			var config = ServiceConfig.Initialize(new ConfigBuilder(options));

			// To display errors in the browser during development, uncomment the following
			// line. Comment it out again when you deploy your service for production use.
			// config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

			Database.SetInitializer(new Initializer());
		}
	}

	public class Initializer : DropCreateDatabaseIfModelChanges<Context>
	{
		protected override void Seed(Context context)
		{
			//List<TodoItem> todoItems = new List<TodoItem>
			//{
			//	new TodoItem { Id = "1", Text = "First item", Complete = false },
			//	new TodoItem { Id = "2", Text = "Second item", Complete = false },
			//};

			//foreach (TodoItem todoItem in todoItems)
			//{
			//	context.Set<TodoItem>().Add(todoItem);
			//}

			base.Seed(context);
		}
	}
}
