using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnapDotNet.Apps.Common
{
	public abstract class NotifyPropertyChangedBase
		: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			field = value;
			NotifyPropertyChanged(propertyName);
		}
	}
}
