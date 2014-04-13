using System;
using System.Collections.ObjectModel;
using SnapDotNet.Core.Snapchat.Models;

namespace SnapDotNet.Apps.ViewModels.SignedIn
{
	public class SnapsViewModel
		: ViewModelBase
	{
		public SnapsViewModel()
		{
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
		}

		public ObservableCollection<Snap> SpoofedSnaps
		{
			get { return _snaps; }
			set { SetField(ref _snaps, value); }
		}
		private ObservableCollection<Snap> _snaps;
	}
}
