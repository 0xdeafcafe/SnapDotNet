using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnapDotNet.Utilities;
using System;
using System.Diagnostics.Contracts;

namespace SnapDotNet.Converters.Json
{
	public sealed class UnixDateTimeConverter
		: DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Timestamps.FromJScriptTime((long) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(Timestamps.ToJScriptTime((DateTime) value));
		}
	}
}