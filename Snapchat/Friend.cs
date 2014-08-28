using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnapDotNet.Utilities;
using SnapDotNet.Responses;
using System.Diagnostics.Contracts;

namespace SnapDotNet
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
		public Friend()
		{
			_stories.CollectionChanged += (sender, args) => OnObservableCollectionChanged(args, "Stories");
		}

		#region Properties

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
		/// Gets or sets the name of this friend.
		/// </summary>
		[JsonProperty]
		public string Name
		{
			get { return _name; }
			set
			{
				SetValue(ref _name, value);
				OnPropertyChanged("FriendlyName");
				OnPropertyChanged("HasDisplayName");
			}
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
		/// Gets if this friend has a <see cref="DisplayName"/>.
		/// </summary>
		[JsonIgnore]
		public bool HasDisplayName
		{
			get { return Name != FriendlyName; }
		}

		/// <summary>
		/// Gets or sets the display name of this friend.
		/// </summary>
		[JsonProperty]
		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				SetValue(ref _displayName, value);
				OnPropertyChanged("FriendlyName");
				OnPropertyChanged("HasDisplayName");
			}
		}
		private string _displayName;

		/// <summary>
		/// Gets or sets the state of the friend request sent to this friend.
		/// </summary>
		[JsonProperty]
		public FriendRequestState FriendRequestState
		{
			get { return _friendRequestState; }
			set { SetValue(ref _friendRequestState, value); }
		}
		private FriendRequestState _friendRequestState;

		/// <summary>
		/// Gets or sets the score of this friend.
		/// </summary>
		[JsonProperty]
		public uint Score
		{
			get { return _score; }
			set { SetValue(ref _score, value); }
		}
		private uint _score;

		/// <summary>
		/// Gets or sets the best friends of this friend.
		/// </summary>
		[JsonProperty]
		public ObservableCollection<string> BestFriends
		{
			get { return _bestFriends; }
			set { SetValue(ref _bestFriends, value); }
		}
		private ObservableCollection<string> _bestFriends = new ObservableCollection<string>();

		/// <summary>
		/// Gets or sets the stories belonging to this friend.
		/// </summary>
		[JsonProperty]
		public ObservableCollection<FriendStory> Stories
		{
			get { return _stories; }
			set { SetValue(ref _stories, value); }
		}
		private ObservableCollection<FriendStory> _stories = new ObservableCollection<FriendStory>();

		/// <summary>
		/// Gets if this friend has any <seealso cref="BestFriends"/>.
		/// </summary>
		[JsonIgnore]
		public bool HasBestFriends
		{
			get { return BestFriends.Any(); }
		}

		#endregion

		#region Update Model Data

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="account"></param>
		public async Task<bool> UpdateFriend(FriendAction action, Account account)
		{
			try
			{
				var data = new Dictionary<string, string>
				{
					{ "username", account.Username },
					{ "action", action.ToString().ToLowerInvariant() },
					{ "friend", Name }
				};

				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("friend", data, account.AuthToken);
				if (response == null || !response.IsLogged)
					throw new InvalidCredentialsException();

				return !String.IsNullOrEmpty(response.Message);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newDisplayName"></param>
		/// <param name="account"></param>
		public async Task<bool> UpdateDisplayName(string newDisplayName, Account account)
		{
			try
			{
				var data = new Dictionary<string, string>
				{
					{"username", account.Username},
					{"action", "display"},
					{"friend", Name},
					{"display", newDisplayName}
				};

				var response = await EndpointManager.Managers["bq"].PostAsync<Response>("friend", data, account.AuthToken);
				if (response == null || !response.IsLogged)
					throw new InvalidCredentialsException();

				var success = !String.IsNullOrEmpty(response.Message);
				if (success)
					DisplayName = newDisplayName;

				return success;
			}
			catch
			{
				return false;
			}
		}

		#endregion
		
		#region Update Model from Responces

		/// <summary>
		/// Update the model based on a <see cref="FriendResponse" />.
		/// </summary>
		/// <param name="friendResponse">The <see cref="FriendResponse" /> to update the Friend model from.</param>
		internal void Update(FriendResponse friendResponse)
		{
			Contract.Requires<ArgumentNullException>(friendResponse != null);

			CanSeeCustomStories = friendResponse.CanSeeCustomStories;
			Name = friendResponse.Name;
			DisplayName = friendResponse.DisplayName;
			FriendRequestState = (FriendRequestState)friendResponse.FriendRequestState;
		}

		/// <summary>
		/// Update the public activity of the model based on a <see cref="PublicActivityResponse" />.
		/// </summary>
		/// <param name="publicActivityResponse">The <see cref="FriendResponse" /> to update the public activity from.</param>
		/// <param name="friends">A list of friends to check</param>
		internal void UpdatePublicActivies(PublicActivityResponse publicActivityResponse, IEnumerable<Friend> friends)
		{
			Contract.Requires<ArgumentNullException>(publicActivityResponse != null || friends != null);

			Score = publicActivityResponse.Score;

			BestFriends.Clear();
			foreach (var bestFriend in publicActivityResponse.BestFriends)
			{
				var friend = friends.FirstOrDefault(f => f.Name == bestFriend);
				BestFriends.Add(friend != null ? friend.FriendlyName : bestFriend);
			}
			OnPropertyChanged("HasBestFriendsName");
		}

		/// <summary>
		/// Update the model's story data based on a <see cref="FriendStoryResponse" />.
		/// </summary>
		/// <param name="friendStoryResponse">The <see cref="FriendStoryResponse" /> to update the Friend model's story data from.</param>
		internal void UpdateStories(FriendStoryResponse friendStoryResponse)
		{
			Contract.Requires<ArgumentNullException>(friendStoryResponse != null);

			// Check new ones, insert them
			foreach (var friendStory in friendStoryResponse.Stories.OrderByDescending(s => s.Story.TimeLeft).Where(friendStory => Stories.FirstOrDefault(s => s.Id == friendStory.Story.Id) == null))
				Stories.Add(FriendStory.Create(friendStory));

			// Check active ones, update them
			foreach (var friendStory in Stories)
			{
				var friendStoryMetadata = friendStoryResponse.Stories.FirstOrDefault(s => s.Story.Id == friendStory.Id);
				if (friendStoryMetadata == null) continue;
				friendStory.Update(friendStoryMetadata);
			}

			// Check expired ones, remove them
			foreach (var redundantStory in Stories.Where(s => friendStoryResponse.Stories.FirstOrDefault(s1 => s1.Story.Id == s.Id) == null || s.Expired))
				Stories.Remove(redundantStory);
		}

		#endregion
		
		/// <summary>
		/// Create a Friend model based on a <see cref="FriendResponse" />.
		/// </summary>
		/// <param name="friendResponse">The <see cref="FriendResponse" /> to create the Friend model from.</param>
		/// <returns></returns>
		[Pure]
		internal static Friend Create(FriendResponse friendResponse)
		{
			Contract.Requires<ArgumentNullException>(friendResponse != null);

			return new Friend
			{
				CanSeeCustomStories = friendResponse.CanSeeCustomStories,
				DisplayName = friendResponse.DisplayName,
				Name = friendResponse.Name,
				FriendRequestState = (FriendRequestState) friendResponse.FriendRequestState
			};
		}
	}
}
