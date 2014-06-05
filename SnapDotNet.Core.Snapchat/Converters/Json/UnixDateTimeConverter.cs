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
			if (reader.TokenType != JsonToken.Integer) 
				return DateTime.UtcNow;

			var converted = Timestamps.ConvertToDateTime((long) reader.Value);
			//if (reader.Path.Contains("replay") || converted.Year < 2013)
			//{
			//	Debug Code, leave this here for tests
			//}
			return converted;
		}

		public override void WriteJson(JsonWriter writer, object value,
			JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteValue(0);
				return;
			}

			if (!(value is DateTime)) throw new Exception("Expected date object value.");

			var epoc = new DateTime(1970, 1, 1);
			var delta = ((DateTime)value) - epoc;
			writer.WriteValue(delta.TotalSeconds < 0 ? 0 : (long) delta.TotalSeconds);
		}
	}
}
