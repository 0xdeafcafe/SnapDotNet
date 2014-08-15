using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SnapDotNet.Converters.Json
{
	public class MillisecondTimeSpanConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var milliseconds = Convert.ToInt64(reader.Value.ToString());
			return TimeSpan.FromMilliseconds(milliseconds);
		}

		public override void WriteJson(JsonWriter writer, object value,
			JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteValue(((TimeSpan)value).TotalMilliseconds);
		}
	}
}
