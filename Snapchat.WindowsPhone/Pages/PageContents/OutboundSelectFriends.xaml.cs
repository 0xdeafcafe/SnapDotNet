using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Snapchat.Models;
using Snapchat.SnapLogic.Api;
using Snapchat.ViewModels.PageContents;
using SnapDotNet.Core.Miscellaneous.Crypto;

namespace Snapchat.Pages.PageContents
{
	public sealed partial class OutboundSelectFriends
	{
		public OutboundSelectFriendsViewModel ViewModel { get; private set; }

		public OutboundSelectFriends()
		{
			InitializeComponent();
		}

		public void Reset()
		{
			DataContext = ViewModel = null;
		}

		public void Load(byte[] imageData)
		{
			DataContext = ViewModel = new OutboundSelectFriendsViewModel(this, imageData);
		}

		#region On Check Changed

		private bool _skipEventLogic;
		private bool _isInMassEvent;

		private void StoryBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			if (_skipEventLogic || _isInMassEvent)
				return;

			var @checked = (bool)(e.OriginalSource as CheckBox).IsChecked;
			var selected = GetCastedDataContext<SelectedOther>(sender);
			if (selected == null) return;

			ViewModel.UpdateSelectedRecipients(selected.OtherName, @checked);
		}

		private void BestFriendsBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			if (_skipEventLogic || _isInMassEvent)
				return;

			// Get Username from Datacontext
			var @checked = (bool) (e.OriginalSource as CheckBox).IsChecked;
			var selected = GetCastedDataContext<SelectedOther>(sender);
			if (selected == null) return;

			DoSelectionLogic(selected.OtherName, SelectionType.BestFriends, @checked);
		}

		private void RecentsBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			if (_skipEventLogic || _isInMassEvent)
				return;

			// Get Username from Datacontext
			var @checked = (bool)(e.OriginalSource as CheckBox).IsChecked;
			var selected = GetCastedDataContext<SelectedOther>(sender);
			if (selected == null) return;

			DoSelectionLogic(selected.OtherName, SelectionType.Recents, @checked);
		}

		private void FriendsBox_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			if (_skipEventLogic || _isInMassEvent)
				return;

			// Get Username from Datacontext
			var @checked = (bool)(e.OriginalSource as CheckBox).IsChecked;
			var selected = GetCastedDataContext<SelectedFriend>(sender);
			if (selected == null) return;

			DoSelectionLogic(selected.Friend.Name, SelectionType.Friends, @checked);
		}

		private void DoSelectionLogic(string username, SelectionType selectionType, bool selectedState)
		{
			_skipEventLogic = true;

			if (selectionType != SelectionType.BestFriends)
				// Select any best friends with the same username
				foreach (var bestFriend in ViewModel.BestFriendsCollection.Where(bestFriend => bestFriend.OtherName == username))
					bestFriend.Selected = selectedState;

			if (selectionType != SelectionType.Recents)
				// Select any best friends with the same username
				foreach (var recent in ViewModel.RecentsCollection.Where(bestFriend => bestFriend.OtherName == username))
					recent.Selected = selectedState;

			if (selectionType != SelectionType.Friends)
				// Select any friends with the same username
				foreach (var recipient in ViewModel.FriendsCollection.SelectMany(recipientGroup => recipientGroup).Where(recipient => recipient.Friend.Name == username))
					recipient.Selected = selectedState;

			ViewModel.UpdateSelectedRecipients(username, selectedState);

			_skipEventLogic = false;
		}

		private void HeaderTextBlock_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			_isInMassEvent = true;

			#region Check our new selection state

			var stuffs = new List<SelectedItem>();
			stuffs.AddRange(ViewModel.StoriesCollection);
			stuffs.AddRange(ViewModel.BestFriendsCollection);
			stuffs.AddRange(ViewModel.RecentsCollection);
			stuffs.AddRange(ViewModel.FriendsCollection.SelectMany(recipientGroup => recipientGroup));

			var newSelectionState = false;
			var hasBeenChecked = false;
			var hasBeenUnChecked = false;

			foreach (var stuff in stuffs)
				if (stuff.Selected)
					hasBeenChecked = true;
				else
					hasBeenUnChecked = true;

			if ((hasBeenChecked && hasBeenUnChecked) || hasBeenUnChecked)
				newSelectionState = true;

			#endregion

			// Stories
			foreach (var story in ViewModel.StoriesCollection)
			{
				story.Selected = newSelectionState;
				ViewModel.UpdateSelectedRecipients(story.OtherName, newSelectionState);
			}

			// Best Friends
			foreach (var bestFriend in ViewModel.BestFriendsCollection)
			{
				bestFriend.Selected = newSelectionState;
				ViewModel.UpdateSelectedRecipients(bestFriend.OtherName, newSelectionState);
			}

			// Recents
			foreach (var recent in ViewModel.RecentsCollection)
			{
				recent.Selected = newSelectionState;
				ViewModel.UpdateSelectedRecipients(recent.OtherName, newSelectionState);
			}

			// Friends
			foreach (var friend in ViewModel.FriendsCollection.SelectMany(recipientGroup => recipientGroup))
			{
				friend.Selected = newSelectionState;
				ViewModel.UpdateSelectedRecipients(friend.Friend.Name, newSelectionState);
			}

			_isInMassEvent = false;
		}

		#endregion

		private async void SelectFriendsButton_OnClick(object sender, RoutedEventArgs e)
		{
			#region Debug Saving xo

			//// write to file
			//var folder = KnownFolders.SavedPictures;
			//var file = await folder.CreateFileAsync("yoloswag.jpg", CreationCollisionOption.ReplaceExisting);

			//using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
			//{
			//	using (var outputStream = fileStream.GetOutputStreamAt(0))
			//	{
			//		using (var dataWriter = new DataWriter(outputStream))
			//		{
			//			dataWriter.WriteBytes(safeData);
			//			await dataWriter.StoreAsync();
			//			dataWriter.DetachStream();
			//		}
			//		await outputStream.FlushAsync();
			//	}
			//}

			#endregion

			// remove stories from recpients
			var hasStorySelected = ViewModel.StoriesCollection.First().Selected;
			if (hasStorySelected)
				ViewModel.SelectedRecipientCollection.Remove(ViewModel.StoriesCollection.First().OtherName);
			
			var mediaId = App.SnapchatManager.GenerateMediaId();
			App.SnapchatManager.SnapchatData.Conversations.Add(new PendingUpload
			{
				ConversationType = ConversationType.PendingUpload,
				Id = mediaId,
				Recipients = ViewModel.SelectedRecipientCollection,
				Status = UploadStatus.Uploading,
				LastInteraction = DateTime.Now
			});
			await App.SnapchatManager.SaveAsync();

			//var dataStream = Aes.EncryptData(ViewModel.ImageData, Convert.FromBase64String(Settings.BlobEncryptionKey));
			//await App.SnapchatManager.Endpoints.UploadMediaAsync(MediaType.Image, mediaId, dataStream);
			//await App.SnapchatManager.Endpoints.SendMediaAsync(mediaId, ViewModel.SelectedRecipientCollection.ToArray());
		}

		private static T GetCastedDataContext<T>(object sender)
			where T : class
		{
			var frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null) return null;

			var dataContext = frameworkElement.DataContext as T;
			return dataContext;
		}
	}
}
