using System;
using SnapDotNet.Apps.Pages;

namespace SnapDotNet.Apps.Attributes
{
	public class RequiresAuthAttribute : Attribute
	{
		public RequiresAuthAttribute(Type redirectPage)
		{
			if (App.SnapChatManager == null ||
			    App.SnapChatManager.IsAuthenticated() == false)
				App.CurrentFrame.Navigate(redirectPage);
		}

		public RequiresAuthAttribute()
			: this(typeof(StartPage)) { }
	}
}
