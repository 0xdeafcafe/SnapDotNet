using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Globalization.Collation;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Snapchat.Models.AppSpecific;

namespace Snapchat.CustomTypes
{
	public class SelectFriendskeyGroup<T> : List<T>
	{
		/// <summary>
		/// The delegate that is used to get the key information.
		/// </summary>
		/// <param name="item">An object of type T</param>
		/// <returns>The key value to use for this object</returns>
		public delegate string GetKeyDelegate(T item);

		/// <summary>
		/// The Key of this group.
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// Public constructor.
		/// </summary>
		/// <param name="key">The key for this group.</param>
		public SelectFriendskeyGroup(string key)
		{
			Key = key;
		}

		/// <summary>
		/// Create a list of AlphaGroup with keys set by a SortedLocaleGrouping.
		/// </summary>
		/// <param name="slg">The </param>
		/// <param name="useCaps">Use capital letters for the friends list.</param>
		/// <returns>Theitems source for a LongListSelector</returns>
		private static List<SelectFriendskeyGroup<T>> CreateGroups(IEnumerable<CharacterGrouping> slg, bool useCaps)
		{
			return (from key in slg where string.IsNullOrWhiteSpace(key.Label) == false select new SelectFriendskeyGroup<T>(useCaps ? key.Label.ToUpperInvariant() : key.Label)).ToList();
		}

		/// <summary>
		/// Create a list of AlphaGroup with keys set by a SortedLocaleGrouping.
		/// </summary>
		/// <param name="items">The items to place in the groups.</param>
		/// <param name="ci">The CultureInfo to group and sort by.</param>
		/// <returns>An items source for a LongListSelector</returns>
		public static ObservableCollection<SelectFriendskeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci)
		{
			var slg = new CharacterGroupings();
			var list = CreateGroups(slg, true);
			list.Insert(0, new SelectFriendskeyGroup<T>("RECENTS"));
			list.Insert(0, new SelectFriendskeyGroup<T>("STORIES"));

			foreach (var item in items)
			{
				string index;

				if (item is SelectedStory)
					index = "STORIES";
				else if (item is SelectedRecent)
					index = "RECENTS";
				else if (item is SelectedFriend)
					index = (item as SelectedFriend).Friend.FriendlyName.ToUpperInvariant()[0].ToString();
				else
					throw new ArgumentException("This item is not of a valid type", "items");

				if (string.IsNullOrEmpty(index) == false)
					list.Find(a => a.Key == index.ToUpperInvariant()).Add(item);
			}

			return new ObservableCollection<SelectFriendskeyGroup<T>>(list);
		}
	}
}
