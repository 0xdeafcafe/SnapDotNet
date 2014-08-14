using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Dialogs
{
	public sealed partial class LogInDialog
	{
#if DEBUG
		public static DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LogInDialog), new PropertyMetadata("wumbotestalex"));
		public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LogInDialog), new PropertyMetadata("testing123"));
#else
		public static DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));
		public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));
#endif

		public LogInDialog()
		{
			InitializeComponent();

			IsPrimaryButtonEnabled = false;
#if DEBUG
			IsPrimaryButtonEnabled = true;
#endif
			SecondaryButtonClick += delegate { Password = String.Empty; };

			username.TextChanged += delegate
			{
				IsPrimaryButtonEnabled = !String.IsNullOrWhiteSpace(username.Text) && !String.IsNullOrWhiteSpace(password.Password);
			};
			password.PasswordChanged += delegate
			{
				IsPrimaryButtonEnabled = !String.IsNullOrWhiteSpace(username.Text) && !String.IsNullOrWhiteSpace(password.Password);
			};
		}

		public string Username
		{
			get { return GetValue(UsernameProperty) as string; }
			set { SetValue(PasswordProperty, value); }
		}

		public string Password
		{
			get { return GetValue(PasswordProperty) as string; }
			set { SetValue(PasswordProperty, value); }
		}

		private void Username_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				password.Focus(FocusState.Keyboard);
		}

		private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				forgotPasswordButton.Focus(FocusState.Keyboard);
		}
	}
}