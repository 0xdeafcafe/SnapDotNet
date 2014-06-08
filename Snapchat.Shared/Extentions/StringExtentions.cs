namespace Snapchat.Extentions
{
	public static class StringExtentions
	{
		public static string ToOrdinal(this int number)
		{
			var work = number.ToString();
			if (number == 11 || number == 12 || number == 13)
				return work + "th";
			switch (number % 10)
			{
				case 1: work += "st"; break;
				case 2: work += "nd"; break;
				case 3: work += "rd"; break;
				default: work += "th"; break;
			}
			return work;
		}
	}
}
