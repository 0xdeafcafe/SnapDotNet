using System.ComponentModel;
using SnapDotNet.Data.ApiResponses;
using SnapDotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SnapDotNet.Data
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
	/// Represents a friend.
	/// </summary>
	public class Friend
		: ObservableObject
	{
		private readonly Account _account;

		internal Friend(Account account)
		{
			_account = account;
			_bestFriends = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>());
			_stories = new ObservableCollection<FriendStory>();
			Stories = new ReadOnlyObservableCollection<FriendStory>(_stories);
		}

		#region Properties

		/// <summary>
		/// Gets the username of this friend.
		/// </summary>
		public string Username
		{
			get { return _username; }
			private set
			{
				if (SetValue(ref _username, value))
					OnPropertyChanged(() => FriendlyName);
			}
		}
		private string _username;

		/// <summary>
		/// Gets a boolean value indicating whether this instance represents the current account.
		/// </summary>
		public bool IsNotCurrentAccount
		{
			get { return _account.Me != this; }
		}

		/// <summary>
		/// Gets the display name of this friend.
		/// </summary>
		public string DisplayName
		{
			get { return _displayName; }
			private set
			{
				if (SetValue(ref _displayName, value))
					OnPropertyChanged(() => FriendlyName);
			}
		}
		private string _displayName;

		/// <summary>
		/// Gets a boolean value indicating whether this friend has a display name.
		/// </summary>
		public bool HasDisplayName
		{
			get { return !string.IsNullOrEmpty(DisplayName); }
		}

		/// <summary>
		/// Gets the <see cref="DisplayName"/> of this friend if one has been set; otherwise, gets
		/// the <see cref="Username"/>.
		/// </summary>
		public string FriendlyName
		{
			get { return HasDisplayName ? DisplayName : Username; }
		}

		/// <summary>
		/// Gets the score of this friend.
		/// </summary>
		public uint Score
		{
			get { return _score; }
			private set { SetValue(ref _score, value); }
		}
		private uint _score;

		/// <summary>
		/// Gets a list of this friend's best friends.
		/// </summary>
		public ReadOnlyObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			private set
			{
				if (SetValue(ref _bestFriends, value))
					OnPropertyChanged(() => HasBestFriends);
			}
		}
		private ReadOnlyObservableCollection<string> _bestFriends;

		/// <summary>
		/// Gets a boolean value indicating whether this friend has best friends.
		/// </summary>
		public bool HasBestFriends
		{
			get { return BestFriends.Any(); }
		}

		/// <summary>
		/// Gets the state of the friend request sent to this friend.
		/// </summary>
		public FriendRequestState FriendRequestState
		{
			get { return _friendRequestState; }
			private set { SetValue(ref _friendRequestState, value); }
		}
		private FriendRequestState _friendRequestState;

		/// <summary>
		/// Gets a collection of stories posted by this friend.
		/// </summary>
		public ReadOnlyObservableCollection<FriendStory> Stories
		{
			get { return _readOnlyStories; }
			private set
			{
				SetValue(ref _readOnlyStories, value); 
				OnPropertyChanged(() => LatestStory);
				OnPropertyChanged(() => IsAnyStoryDownloading);
			}
		}
		private ReadOnlyObservableCollection<FriendStory> _readOnlyStories;
		private readonly ObservableCollection<FriendStory> _stories;

		/// <summary>
		/// Gets the latest story posted by this friend.
		/// </summary>
		public FriendStory LatestStory
		{
			get { return Stories.FirstOrDefault(); }
		}

		/// <summary>
		/// Gets a boolean value indicating whether the thumbnail for this friend in the story
		/// list shouldn't "decay". In other words, the degree of the thumbnail is always at 360*.
		/// </summary>
		public bool DontDecayThumbnail
		{
			get { return _dontDecayThumbnail; }
			private set { SetValue(ref _dontDecayThumbnail, value); }
		}
		private bool _dontDecayThumbnail;

		/// <summary>
		/// Gets a boolean value indicating whether any story posted by this user is currently
		/// being downloaded.
		/// </summary>
		public bool IsAnyStoryDownloading
		{
			get { return Stories.Any(friend => friend.IsDownloading); }
		}

		/*/// <summary>
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
		/// Gets or sets if this friend contains mature content.
		/// </summary>
		[JsonProperty]
		public bool ContainsMatureContent
		{
			get { return _contiansMatureContent; }
			set { SetValue(ref _contiansMatureContent, value); }
		}
		private bool _contiansMatureContent;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public DateTime Expiration
		{
			get { return _expiration; }
			set { SetValue(ref _expiration, value); }
		}
		private DateTime _expiration;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string SharedStoryId
		{
			get { return _sharedStoryId; }
			set { SetValue(ref _sharedStoryId, value); }
		}
		private string _sharedStoryId;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public bool IsSharedStory
		{
			get { return _isSharedStory; }
			set { SetValue(ref _isSharedStory, value); }
		}
		private bool _isSharedStory;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public bool HasCustomDescription
		{
			get { return _hasCustomDescription; }
			set { SetValue(ref _hasCustomDescription, value); }
		}
		private bool _hasCustomDescription;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty]
		public string Venue
		{
			get { return _venue; }
			set { SetValue(ref _venue, value); }
		}
		private string _venue;
*/

		#endregion

		#region Friend Actions

		/// <summary>
		/// Downloads all stories posted by this friend.
		/// </summary>
		public async Task DownloadStoriesAsync()
		{
			foreach (var story in Stories)
				await story.DownloadMediaAsync();

			OnPropertyChanged(() => IsAnyStoryDownloading);
		}

		/// <summary>
		/// Sets this friend's display name.
		/// </summary>
		/// <param name="displayName">The friend's new display name.</param>
		/// <returns><c>true</c> if set successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> SetDisplayNameAsync(string displayName)
		{
			try
			{
				var data = new Dictionary<string, string>
				{
					{ "username", _account.Username },
					{ "action", "display" },
					{ "friend", Username },
					{ "display", displayName }
				};

				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("friend", data, _account.AuthToken);
				if (response.IsLogged)
				{
					Debug.WriteLine("[Friend] Display name for {0} set to {1}", Username, displayName);
					DisplayName = displayName;
					return true;
				}
				else
				{
					Debug.WriteLine("[Friend] Failed to set display name for {0}. Reason: {1}", Username, response.Message);
					return false;
				}
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		/// <summary>
		/// Add this friend to the user's friends list.
		/// </summary>
		/// <returns><c>true</c> if added successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> AddAsync()
		{
			bool added = await InvokeFriendActionAsync("add");
			if (added)
			{
				// TODO: Add to Account.Friends
			}
			return added;
		}

		/// <summary>
		/// Remove this friend from the user's friends list.
		/// </summary>
		/// <returns><c>true</c> if deleted successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> DeleteAsync()
		{
			bool deleted = await InvokeFriendActionAsync("delete");
			if (deleted)
			{
				_account._friends.Remove(this);
			}
			return deleted;
		}

		/// <summary>
		/// Block this friend.
		/// </summary>
		/// <returns><c>true</c> if blocked successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> BlockAsync()
		{
			bool blocked = await InvokeFriendActionAsync("block");
			if (blocked)
			{
				FriendRequestState = FriendRequestState.Blocked;
				// TODO: Add to Account.BlockedFriends
			}
			return blocked;
		}

		/// <summary>
		/// Unblock this friend.
		/// </summary>
		/// <returns><c>true</c> if unblocked successfully; otherwise, <c>false</c>.</returns>
		public async Task<bool> UnblockAsync()
		{
			var unblocked = await InvokeFriendActionAsync("unblock");
			if (unblocked)
			{
				FriendRequestState = FriendRequestState.Accepted;
				// TODO: Remove from Account.BlockedFriends and add to Account.Friends
			}
			return unblocked;
		}

		/// <summary>
		/// Invokes an action on this friend.
		/// </summary>
		/// <param name="action">The name of the action to invoke.</param>
		/// <returns><c>true</c> if invoked successfully; otherwise, <c>false</c>.</returns>
		private async Task<bool> InvokeFriendActionAsync(string action)
		{
			try
			{
				var data = new Dictionary<string, string>
				{
					{ "username", _account.Username },
					{ "action", action },
					{ "friend", Username },
				};

				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("friend", data, _account.AuthToken);
				if (response.IsLogged)
				{
					Debug.WriteLine("[Friend] Invoked action {1} on {0}", Username, action);
					return true;
				}
				else
				{
					Debug.WriteLine("[Friend] Failed to invoke action {1} on {0}. Reason: {2}", Username, action, response.Message);
					return false;
				}
			}
			catch (InvalidHttpResponseException ex)
			{
				if (ex.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new InvalidCredentialsException();

				throw;
			}
		}

		#endregion

		/// <summary>
		/// Create an instance of the <see cref="Friend"/> class from the given <paramref name="response"/>.
		/// </summary>
		/// <param name="account">An <see cref="Account"/> instance.</param>
		/// <param name="response">A <see cref="FriendResponse"/> instance.</param>
		/// <returns>
		/// A <see cref="Friend"/> instance reflecting the values in the <paramref name="response"/>.
		/// </returns>
		[Pure]
		internal static Friend CreateFromResponse(Account account, FriendResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			var friend = new Friend(account);
			friend.UpdateFromResponse(response);
			return friend;
		}

		/// <summary>
		/// Updates the properties in this instance to reflect the values in the given
		/// <paramref name="response"/>.
		/// </summary>
		/// <param name="response">>An <see cref="FriendResponse"/> instance.</param>
		internal void UpdateFromResponse(FriendResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			/*  CanSeeCustomStories = friendResponse.CanSeeCustomStories,
				Expiration = friendResponse.Expiration,
				SharedStoryId = friendResponse.SharedStoryId,
				IsSharedStory = friendResponse.IsSharedStory,
				HasCustomDescription = friendResponse.HasCustomDescription,
				Venue = friendResponse.Venue*/

			Username = response.Name;
			DisplayName = response.DisplayName;
			FriendRequestState = (FriendRequestState) response.FriendRequestState;
			DontDecayThumbnail = response.DontDecayThumbnail;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response">An <see cref="FriendStoryResponse"/> instance.</param>
		internal void UpdateStoriesFromResponse(FriendStoryResponse response)
		{
			Contract.Requires<ArgumentNullException>(response != null);

			// Add new stories.
			foreach (
				var friendStory in
					response.Stories.OrderByDescending(s => s.Story.TimeLeft)
						.Where(friendStory => Stories.FirstOrDefault(s => s.Id == friendStory.Story.Id) == null))
			{
				var newStory = FriendStory.CreateFromResponse(this, friendStory);
				newStory.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(() => IsAnyStoryDownloading);
				_stories.Add(newStory);
			}

			// Check for active ones, and update them.
			foreach (var friendStory in Stories)
			{
				var friendStoryMetadata = response.Stories.FirstOrDefault(s => s.Story.Id == friendStory.Id);
				if (friendStoryMetadata == null) continue;
				friendStory.UpdateFromResponse(friendStoryMetadata);
			}

			// Remove expired stories.
			foreach (var redundantStory in Stories.Where(s => response.Stories.FirstOrDefault(s1 => s1.Story.Id == s.Id) == null || s.IsExpired).ToList())
				_stories.Remove(redundantStory);

			OnPropertyChanged(() => LatestStory);
			OnPropertyChanged(() => IsAnyStoryDownloading);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <param name="friends"></param>
		internal void UpdatePublicActivity(PublicActivityResponse response, IEnumerable<Friend> friends)
		{
			Contract.Requires<ArgumentNullException>(response != null && friends != null);

			Score = response.Score;
			BestFriends = new ReadOnlyObservableCollection<string>(response.BestFriends);
		}
	}
}
