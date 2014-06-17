using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Snapchat.Helpers
{
	public static class VariousHelpers
	{
		public static T FindVisualChild<T>(DependencyObject obj)
			where T : DependencyObject
		{
			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				var child = VisualTreeHelper.GetChild(obj, i);
				if (child is T)
					return (T)child;

				var childOfChild = FindVisualChild<T>(child);
				if (childOfChild != null)
					return childOfChild;
			}
			return null;
		}
	}
}
