﻿using SnapDotNet.Apps.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SnapDotNet.Apps.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			DataContext = new MainViewModel();
			this.InitializeComponent();
			this.SizeChanged += (object sender, SizeChangedEventArgs e) =>
			{
				if (e.NewSize.Width <= (int) Resources["MinimalViewMaxWidth"])
					VisualStateManager.GoToState(this, "MinimalLayout", true);
				else if (e.NewSize.Width < e.NewSize.Height)
					VisualStateManager.GoToState(this, "PortraitLayout", true);
				else
					VisualStateManager.GoToState(this, "DefaultLayout", true);
			};
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested += OnSettingsCommandsRequested;
			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			SettingsPane.GetForCurrentView().CommandsRequested -= OnSettingsCommandsRequested;
			base.OnNavigatedFrom(e);
		}

		void OnSettingsCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			args.Request.ApplicationCommands.Add(new SettingsCommand("account", "Account", delegate
			{
				// TODO: Open account settings flyout
			}));

			args.Request.ApplicationCommands.Add(new SettingsCommand("sign_out", "Sign out", delegate
			{
				// TODO: Sign out
				App.CurrentFrame.Navigate(typeof(StartPage));
			}));

			args.Request.ApplicationCommands.Add(new SettingsCommand("privacy_policy", "Privacy policy", delegate
			{
				// TODO: Open privacy policy
			}));
		}

		private void OnSnapSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// This is annoying as hell
			/*(if (e.AddedItems.Count > 0)
			{
				this.BottomAppBar.IsOpen = true;
				this.BottomAppBar.IsSticky = true;
			}
			else
			{
				this.BottomAppBar.IsOpen = false;
				this.BottomAppBar.IsSticky = false;
			}*/
		}

		private void OnBottomAppBarHintEntered(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0x91, 0x11));
		}

		private void OnBottomAppBarHintExited(object sender, PointerRoutedEventArgs e)
		{
			(sender as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00));
		}

		private void OnBottomAppBarHintTapped(object sender, TappedRoutedEventArgs e)
		{
			this.BottomAppBar.IsOpen = true;
		}
	}
}