using ColdSnap.Common;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Dialogs
{
	public sealed partial class LogInDialog : ContentDialog
	{
		public static DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));
		public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LogInDialog), new PropertyMetadata(String.Empty));

		public LogInDialog()
		{
			InitializeComponent();

			IsPrimaryButtonEnabled = false;
			SecondaryButtonClick += delegate { Password = String.Empty; };

			this.username.TextChanged += delegate
			{
				IsPrimaryButtonEnabled = !String.IsNullOrWhiteSpace(this.username.Text) && !String.IsNullOrWhiteSpace(this.password.Password);
			};
			this.password.PasswordChanged += delegate
			{
				IsPrimaryButtonEnabled = !String.IsNullOrWhiteSpace(this.username.Text) && !String.IsNullOrWhiteSpace(this.password.Password);
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
				this.password.Focus(Windows.UI.Xaml.FocusState.Keyboard);
		}

		private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				this.forgotPasswordButton.Focus(Windows.UI.Xaml.FocusState.Keyboard);
		}
	}
}