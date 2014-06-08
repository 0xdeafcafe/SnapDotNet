﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Globalization.Collation;

namespace SnapDotNet.Core.Miscellaneous.CustomTypes
{
	public class AlphaKeyGroup<T> : List<T>
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
		public AlphaKeyGroup(string key)
		{
			Key = key;
		}

		/// <summary>
		/// Create a list of AlphaGroup with keys set by a SortedLocaleGrouping.
		/// </summary>
		/// <param name="slg">The </param>
		/// <returns>Theitems source for a LongListSelector</returns>
		private static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<CharacterGrouping> slg)
		{
			return (from key in slg where string.IsNullOrWhiteSpace(key.Label) == false select new AlphaKeyGroup<T>(key.Label)).ToList();
		}

		/// <summary>
		/// Create a list of AlphaGroup with keys set by a SortedLocaleGrouping.
		/// </summary>
		/// <param name="items">The items to place in the groups.</param>
		/// <param name="ci">The CultureInfo to group and sort by.</param>
		/// <param name="getKey">A delegate to get the key from an item.</param>
		/// <param name="sort">Will sort the data if true.</param>
		/// <returns>An items source for a LongListSelector</returns>
		public static ObservableCollection<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort)
		{
			var slg = new CharacterGroupings();
			var list = CreateGroups(slg);

			foreach (var item in items)
			{
				var index = slg.Lookup(getKey(item));
				if (string.IsNullOrEmpty(index) == false)
				{
					list.Find(a => a.Key == index).Add(item);
				}
			}

			if (!sort) 
				return new ObservableCollection<AlphaKeyGroup<T>>(list);

			foreach (var group in list)
				@group.Sort((c0, c1) => ci.CompareInfo.Compare(getKey(c0), getKey(c1)));

			return new ObservableCollection<AlphaKeyGroup<T>>(list);
		}

	}
}
