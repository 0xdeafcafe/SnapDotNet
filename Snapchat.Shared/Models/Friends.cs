using System;
using System.Runtime.Serialization;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.Models
{
	public class AddedFriend
		: NotifyPropertyChangedBase
	{
		public String Direction
		{
			get { return _direction; }
			set { SetField(ref _direction, value); }
		}
		private String _direction;

		public String Display
		{
			get { return _display; }
			set { SetField(ref _display, value); }
		}
		private String _display;

		public String Name
		{
			get { return _name; }
			set { SetField(ref _name, value); }
		}
		private String _name;

		public DateTime AddedAt
		{
			get { return _addedAt; }
			set { SetField(ref _addedAt, value); }
		}
		private DateTime _addedAt;

		public FriendRequestState Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private FriendRequestState _type;

		#region Helpers

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		#endregion
	}

	public class Friend
		: NotifyPropertyChangedBase
	{
		public Boolean CanSeeCustomStories
		{
			get { return _canSeeCustomStories; }
			set { SetField(ref _canSeeCustomStories, value); }
		}
		private Boolean _canSeeCustomStories;

		public String Direction
		{
			get { return _direction; }
			set { SetField(ref _direction, value); }
		}
		private String _direction;

		public String Display
		{
			get { return _display; }
			set
			{
				SetField(ref _display, value);
				NotifyPropertyChanged("Name");
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("HasFriendlyName");
			}
		}
		private String _display;

		public String Name
		{
			get { return _name; }
			set
			{
				SetField(ref _name, value);
				NotifyPropertyChanged("Display");
				NotifyPropertyChanged("FriendlyName");
				NotifyPropertyChanged("HasFriendlyName");
			}
		}
		private String _name;

		public FriendRequestState Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private FriendRequestState _type;

		public PublicActivity PublicActivity
		{
			get { return _publicActivity; }
			set { SetField(ref _publicActivity, value); }
		}
		private PublicActivity _publicActivity = new PublicActivity();

		#region Helpers

		[IgnoreDataMember]
		public String FriendlyName { get { return String.IsNullOrWhiteSpace(Display) ? Name : Display; } }

		[IgnoreDataMember]
		public Boolean HasFriendlyName { get { return FriendlyName == Display; } }

		#endregion
	}

	/// <summary>
	/// Actions concerning other snapchat users
	/// </summary>
	public enum FriendAction
	{
		Add,
		Delete,
		Block,
		Unblock
	}

	/// <summary>
	/// Indicates the state of a friend request.
	/// </summary>
	public enum FriendRequestState
	{
		Accepted,
		Pending,
		Blocked
	}

	/// <summary>
	/// Indicates an account's privacy mode.
	/// </summary>
	public enum AccountPrivacy
	{
		/// <summary>
		/// Everyone can send snaps to the account.
		/// </summary>
		Everyone,

		/// <summary>
		/// Only friends can send snaps to the account.
		/// </summary>
		Friends
	}

	/// <summary>
	/// Indicates an account's privacy mode.
	/// </summary>
	public enum StoryPrivacy
	{
		/// <summary>
		/// Everyone can see the story.
		/// </summary>
		Everyone,

		/// <summary>
		/// Only friends can see the story.
		/// </summary>
		Friends,

		/// <summary>
		/// Only friends can see the story, but the user can block certain friends.
		/// </summary>
		Custom
	}
}
