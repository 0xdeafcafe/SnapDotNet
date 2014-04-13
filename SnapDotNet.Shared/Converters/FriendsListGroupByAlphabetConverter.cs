using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;
using SnapDotNet.Core.Snapchat.Models;
using SnapDotNet.Core.Miscellaneous.Models;

namespace SnapDotNet.Apps.Converters
{
	public class FriendsListGroupByAlphabetConverter : IValueConverter
	{
		private const string Alphabet = "#abcdefghijklmnopqrstuvwxyz~";

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var friends = value as ObservableCollection<Friend>;
			if (friends == null) return null;
			
			var groups = Alphabet.ToLowerInvariant().Select(alpha => new GroupInfoList<object> {Key = alpha}).ToList();
			foreach (var friend in friends.Where(f => f.FriendRequestState == Filter))
			{
				var startingChar = friend.FriendlyName[0];
				if (Char.IsDigit(startingChar))
					groups.First(g => (string) g.Key == "#").Add(friend);
				else if (!Char.IsLetter(startingChar))
					groups.First(g => (string) g.Key == "~").Add(friend);
				else
					groups.First(g => (char) g.Key == Char.ToLowerInvariant(startingChar)).Add(friend);
			}
			foreach (var group in groups)
				group.KeyIsVisible = group.Any();

			return groups;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public FriendRequestState Filter { get; set; }
	}
}
