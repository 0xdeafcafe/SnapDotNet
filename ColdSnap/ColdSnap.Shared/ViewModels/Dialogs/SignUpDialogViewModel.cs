using ColdSnap.Common;
using System;

namespace ColdSnap.ViewModels.Dialogs
{
	public sealed class SignUpDialogViewModel
		: ObservableObject
	{
		public const int MinimumAge = 13;

		/// <summary>
		/// Gets the maximum year for the birthday field based on the minimum age requirement.
		/// </summary>
		public DateTimeOffset MaxBirthdayYear
		{
			get
			{
				return new DateTimeOffset(new DateTime(DateTime.Today.Year - MinimumAge, DateTime.Today.Month, DateTime.Today.Day));
			}
		}
	}
}
