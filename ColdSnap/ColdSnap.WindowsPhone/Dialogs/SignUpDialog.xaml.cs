using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ColdSnap.Dialogs
{
	public sealed partial class SignUpDialog
	{
		public static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(SignUpDialog), new PropertyMetadata(String.Empty));
		public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(SignUpDialog), new PropertyMetadata(String.Empty));

		public SignUpDialog()
		{
			InitializeComponent();
			IsPrimaryButtonEnabled = false;
			SecondaryButtonClick += delegate
			{
				Password = String.Empty;
				Email = String.Empty;
			};
			Action togglePrimaryButton = () => IsPrimaryButtonEnabled = !String.IsNullOrWhiteSpace(email.Text) && !String.IsNullOrWhiteSpace(password.Password);
			email.TextChanged += delegate { togglePrimaryButton(); };
			password.PasswordChanged += delegate { togglePrimaryButton(); };
		}

		public string Email
		{
			get { return GetValue(EmailProperty) as string; }
			set { SetValue(EmailProperty, value); }
		}

		public string Password
		{
			get { return GetValue(PasswordProperty) as string; }
			set { SetValue(PasswordProperty, value); }
		}

		private void Email_OnKeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				password.Focus(FocusState.Keyboard);
		}

		private void Password_OnKeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
				birthday.Focus(FocusState.Keyboard);
		}
	}
}