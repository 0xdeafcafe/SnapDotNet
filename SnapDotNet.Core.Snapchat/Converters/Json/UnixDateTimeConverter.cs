using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnapDotNet.Core.Snapchat.Helpers;

namespace SnapDotNet.Core.Snapchat.Converters.Json
{
	public class UnixDateTimeConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			return reader.TokenType != JsonToken.Integer ? DateTime.UtcNow : Timestamps.ConvertToDateTime((long) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value,
			JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			long ticks;
			if (value is DateTime)
			{
				var epoc = new DateTime(1970, 1, 1);
				var delta = ((DateTime)value) - epoc;
				if (delta.TotalSeconds < 0)
					throw new ArgumentOutOfRangeException("value", "Unix epoc starts January 1st, 1970");

				ticks = (long)delta.TotalSeconds;
			}
			else
				throw new Exception("Expected date object value.");

			writer.WriteValue(ticks);
		}
	}
}
