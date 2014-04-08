using SnapDotNet.Apps.Common;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace SnapDotNet.Apps.ViewModels
{
	public sealed class StartViewModel
		: NotifyPropertyChangedBase
	{
		public StartViewModel()
		{
			IsStartPageVisible = true;

			#region Register Commands

			OpenSignInPageCommand = new RelayCommand(
				() =>
				{
					IsSignInPageVisible = true;
					IsStartPageVisible = false;
				},
				() => IsStartPageVisible);

			OpenRegisterPageCommand = new RelayCommand(
				() =>
				{
					IsRegisterPageVisible = true;
					IsStartPageVisible = false;
				},
				() => IsStartPageVisible);

			GoBackToStartCommand = new RelayCommand(() =>
			{
				IsSignInPageVisible = false;
				IsRegisterPageVisible = false;
				IsStartPageVisible = true;
			});

			SignInCommand = new RelayCommand<Page>(SignIn);
			RegisterCommand = new RelayCommand<Page>(Register);

			#endregion
		}

		/// <summary>
		/// Gets the command to open the sign in form.
		/// </summary>
		public ICommand OpenSignInPageCommand
		{
			get { return _openSignInPageCommand; }
			private set { SetField(ref _openSignInPageCommand, value); }
		}
		private ICommand _openSignInPageCommand;

		/// <summary>
		/// Gets the command to open the registration form.
		/// </summary>
		public ICommand OpenRegisterPageCommand
		{
			get { return _openRegisterPageCommand; }
			private set { SetField(ref _openRegisterPageCommand, value); }
		}
		private ICommand _openRegisterPageCommand;

		/// <summary>
		/// Gets the command to go back to the start page.
		/// </summary>
		public ICommand GoBackToStartCommand
		{
			get { return _goBackToStartCommand; }
			private set { SetField(ref _goBackToStartCommand, value); }
		}
		private ICommand _goBackToStartCommand;

		/// <summary>
		/// Gets the command to sign in.
		/// </summary>
		public ICommand SignInCommand
		{
			get { return _signInCommand; }
			private set { SetField(ref _signInCommand, value); }
		}
		private ICommand _signInCommand;

		/// <summary>
		/// Gets the command to register.
		/// </summary>
		public ICommand RegisterCommand
		{
			get { return _registerCommand; }
			private set { SetField(ref _registerCommand, value); }
		}
		private ICommand _registerCommand;

		/// <summary>
		/// Gets whether the sign in form should be visible.
		/// </summary>
		public bool IsSignInPageVisible
		{
			get { return _isSignInPageVisible; }
			private set
			{
				SetField(ref _isSignInPageVisible, value);
			}
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

		/// <summary>
		/// Gets whether the starting menu should be visible.
		/// </summary>
		public bool IsStartPageVisible
		{
			get { return _isStartPageVisible; }
			private set { SetField(ref _isStartPageVisible, value); }
		}
		private bool _isStartPageVisible;

		/// <summary>
		/// Attempts to sign the user into Snapchat.
		/// </summary>
		private void SignIn(Page nextPage)
		{
			// TODO: API stuff
			App.CurrentFrame.Navigate(nextPage.GetType());
		}

		/// <summary>
		/// Attempts to create a new account.
		/// </summary>
		private void Register(Page nextPage)
		{
			// TODO: API stuff
			App.CurrentFrame.Navigate(nextPage.GetType());
		}
	}
}
