using System;
using Newtonsoft.Json;
using SnapDotNet.Utilities;

namespace SnapDotNet.Models
{

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
	/// Represents a friend.
	/// </summary>
	public sealed class Friend
		: ObservableObject
	{
		/// <summary>
		/// Gets or sets whether this friend allows you to see custom stories.
		/// </summary>
		[JsonProperty]
		public bool CanSeeCustomStories
		{
			get { return _canSeeCustomStories; }
			set { SetValue(ref _canSeeCustomStories, value); }
		}
		private bool _canSeeCustomStories;

		/// <summary>
		/// Gets or sets the name of this friend.
		/// </summary>
		[JsonProperty]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, value); }
		}
		private string _name;

		/// <summary>
		/// Gets the Display Name, and if onehasn't been set, falls back to the Name, for this friend.
		/// </summary>
		[JsonIgnore]
		public string FriendlyName
		{
			get { return String.IsNullOrEmpty(DisplayName) ? Name : DisplayName; }
		}

		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[JsonProperty]
		public string DisplayName
		{
			get { return _displayName; }
			set { SetValue(ref _displayName, value); }
		}
		private string _displayName;

		/// <summary>
		/// Gets or sets the state of the friend request sent to this person.
		/// </summary>
		[JsonProperty]
		public FriendRequestState FriendRequestState
		{
			get { return _friendRequestState; }
			set { SetValue(ref _friendRequestState, value); }
		}
		private FriendRequestState _friendRequestState;
	}
}
