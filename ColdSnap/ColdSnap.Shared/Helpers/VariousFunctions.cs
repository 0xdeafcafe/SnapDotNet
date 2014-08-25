using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ColdSnap.Helpers
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

		public static FrameworkElement FindDescendantByName(this FrameworkElement element, string name)
		{
			if (element == null || string.IsNullOrWhiteSpace(name)) { return null; }

			if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
			{
				return element;
			}
			var childCount = VisualTreeHelper.GetChildrenCount(element);
			for (var i = 0; i < childCount; i++)
			{
				var result = (VisualTreeHelper.GetChild(element, i) as FrameworkElement).FindDescendantByName(name);
				if (result != null) { return result; }
			}
			return null;
		}
	}
}