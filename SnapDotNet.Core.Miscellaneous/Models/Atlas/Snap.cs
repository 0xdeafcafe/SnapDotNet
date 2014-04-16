using System;

namespace SnapDotNet.Core.Miscellaneous.Models.Atlas
{
	public class Snap
	{
		public string Id { get; set; }

		public int UserId { get; set; }

		public string SnapId { get; set; }

		public string SnapSenderUsername { get; set; }

		public int SnapStatus { get; set; }
	}
}