using SnapDotNet.Apps.Common;
using System;
using System.Windows.Input;

namespace SnapDotNet.Apps.ViewModels
{
    public sealed class StartViewModel
		: NotifyPropertyChangedBase
    {
		public StartViewModel()
		{
			IsSignInPageVisible = true;

			OpenSignInPageCommand = new RelayCommand(() =>
			{
				IsSignInPageVisible = true;
			});

			GoBackToStartCommand = new RelayCommand(() =>
			{
				IsSignInPageVisible = false;
				IsRegisterPageVisible = false;
			});
		}

		/// <summary>
		/// Gets or sets the command to open the sign in page.
		/// </summary>
		public ICommand OpenSignInPageCommand
		{
			get { return _openSignInPageCommand; }
			set { SetField(ref _openSignInPageCommand, value); }
		}
		private ICommand _openSignInPageCommand;

		/// <summary>
		/// Gets or sets the command to go back to the start page.
		/// </summary>
		public ICommand GoBackToStartCommand
		{
			get { return _goBackToStartCommand; }
			set { SetField(ref _goBackToStartCommand, value); }
		}
		private ICommand _goBackToStartCommand;

		/// <summary>
		/// Gets whether the sign in form should be visible.
		/// </summary>
		public bool IsSignInPageVisible
		{
			get { return _isSignInPageVisible; }
			private set { SetField(ref _isSignInPageVisible, value); }
		}
		private bool _isSignInPageVisible;

		/// <summary>
		/// Gets whether the registration form should be visible.
		/// </summary>
		public bool IsRegisterPageVisible
		{
			get { return _isRegisterPageVisible; }
			private set { SetField(ref _isRegisterPageVisible, value); }
		}
		private bool _isRegisterPageVisible;
    }
}
