using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Converters.Json
{
	public class StoryPrivacyToEnumConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			switch (reader.Value.ToString().ToUpper())
			{
				case "FRIENDS":
					return StoryPrivacy.Friends;
				case "EVERYONE":
					return StoryPrivacy.Everyone;
				default:
					return StoryPrivacy.Custom;
			}
		}

		public override void WriteJson(JsonWriter writer, object value,
			JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteValue(value.ToString().ToUpper());
		}
	}
}
