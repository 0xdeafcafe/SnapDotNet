﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapDotNet.Core.Miscellaneous.Helpers
{
	public static class Time
	{
		/// <summary>
		/// </summary>
		/// <param name="theDate"></param>
		/// <returns></returns>
		public static string GetReleativeDate(DateTime theDate)
		{
			const int minute = 60;
			const int hour = 60 * minute;
			const int day = 24 * hour;
			var thresholds = new Dictionary<long, string>
			{
				{60, "{0} seconds ago"},
				{minute*2, "a minute ago"},
				{45*minute, "{0} minutes ago"},
				{120*minute, "an hour ago"},
				{day, "{0} hours ago"},
				{day*2, "yesterday"},
				{day*30, "{0} days ago"}
			};

			var since = (DateTime.UtcNow.Ticks - theDate.Ticks) / 10000000;
			return (from threshold in thresholds.Keys
					where since < threshold
					let t = new TimeSpan((DateTime.UtcNow.Ticks - theDate.Ticks))
					select
						string.Format(thresholds[threshold],
							(t.Days > 365
								? t.Days / 365
								: (t.Days > 0
									? t.Days
									: (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0)))))))
				.FirstOrDefault();
		}
	}
}