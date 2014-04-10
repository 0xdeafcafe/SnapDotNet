using System.Collections.Generic;

namespace SnapDotNet.Core.Miscellaneous.Models
{
	public class GroupInfoList<T> : List<object>
	{
		public object Key { get; set; }

		public bool KeyIsVisible { get; set; }

		public new IEnumerator<object> GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}
}
