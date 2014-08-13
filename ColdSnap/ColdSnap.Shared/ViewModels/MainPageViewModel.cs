using ColdSnap.Common;
using SnapDotNet;
using System;
using System.Threading.Tasks;

namespace ColdSnap.ViewModels
{
	public sealed class MainPageViewModel
		: ObservableObject
	{
		public Account Account
		{
			get { return _account; }
			set { SetValue(ref _account, value); }
		}
		private Account _account;
    }
}
