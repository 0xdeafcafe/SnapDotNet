namespace ColdSnap.ViewModels
{
	public class ConversationPageViewModel
		: BaseViewModel
	{
		public string Title
		{
			get { return _title; }
			set { SetValue(ref _title, value); }
		}
		private string _title;
	}
}