using System.Collections.Generic;
using Windows.UI;

namespace SnapDotNet.Utilities.CustomTypes
{
	public interface IAlphaKeyGroup<T> : IList<T>
	{
		string Key { get; }

		string FriendlyKey { get; }

		Color BackgroundColour { get; }

		Color ForegroundColour { get; }
	}
}
