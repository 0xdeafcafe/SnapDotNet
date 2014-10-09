using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnapDotNet.Utilities;
using System;

namespace SnapDotNet.Converters.Json
{
	internal sealed class UnixDateTimeConverter
		: DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Timestamps.FromJScriptTime((long) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((DateTime) value).ToJScriptTime());
		}
	}
}