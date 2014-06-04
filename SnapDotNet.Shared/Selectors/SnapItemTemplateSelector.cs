using SnapDotNet.Core.Snapchat.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SnapDotNet.Apps.Selectors
{
    public sealed class SnapItemTemplateSelector
		: DataTemplateSelector
    {
		public DataTemplate CountdownTemplate { get; set; }
		public DataTemplate DefaultTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			var snap = item as Snap;

			if (snap.Status == SnapStatus.Opened && snap.SenderName != App.SnapchatManager.Account.Username)
				return CountdownTemplate;

			return DefaultTemplate;
		}
    }
}
