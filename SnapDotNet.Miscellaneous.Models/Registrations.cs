using Newtonsoft.Json;

namespace SnapDotNet.Core
{
	public class Registrations
	{
		public string Id { get; set; }

		[JsonProperty(PropertyName = "handle")]
		public string Handle { get; set; }
	}
}
