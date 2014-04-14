using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SnapDotNet.Apps.Common;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class SnapsViewModel
		: ViewModelBase
	{
		public SnapsViewModel()
		{
			#region Create Spoofed Test Snaps

			SpoofedSnaps = new ObservableCollection<Snap>
			{
				// Sent
				new Snap // Delivered
				{
					SenderName = "wumbotestalex",
					RecipientName = "Snaisy",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Delivered,
					MediaType = MediaType.Image
				},
				new Snap // Opened
				{
					SenderName = "wumbotestalex",
					RecipientName = "Snaisy",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Opened,
					MediaType = MediaType.Image
				},
				new Snap // Screenshot
				{
					SenderName = "wumbotestalex",
					RecipientName = "Snaisy",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Screenshotted,
					MediaType = MediaType.Image
				},

				// Recieved
				new Snap // Delivered
				{
					SenderName = "Snaisy",
					RecipientName = "wumbotestalex",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Delivered,
					MediaType = MediaType.Image
				},
				new Snap // Opened
				{
					SenderName = "Snaisy",
					RecipientName = "wumbotestalex",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Opened,
					MediaType = MediaType.Image
				},
				new Snap // Screenshotted
				{
					SenderName = "Snaisy",
					RecipientName = "wumbotestalex",
					Timestamp = DateTime.UtcNow,
					Status = SnapStatus.Screenshotted,
					MediaType = MediaType.Image
				}
			};

			#endregion

			TryDownloadMediaCommand = new RelayCommand<Snap>(async snap =>
			{
				await snap.DownloadSnapBlob(App.SnapChatManager);
			});
		}

		public ICommand TryDownloadMediaCommand
		{
			get { return _tryDownloadMediaCommand; }
			set { SetField(ref _tryDownloadMediaCommand, value); }
		}
		private ICommand _tryDownloadMediaCommand;

		public ObservableCollection<Snap> SpoofedSnaps
		{
			get { return _snaps; }
			set { SetField(ref _snaps, value); }
		}
		private ObservableCollection<Snap> _snaps;
	}
}
