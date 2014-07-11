using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Snapchat.Models;
using Snapchat.SnapLogic.Converters.Json;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class Snap
		: NotifyPropertyChangedBase, IComparable, IComparable<Snap>
	{
		public Snap()
		{
			Status = SnapStatus.None;
		}

		[DataMember(Name = "c_id")]
		public string ContentId { get; set; }

		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "m")]
		public MediaType MediaType { get; set; }

		[DataMember(Name = "rp")]
		public string RecipientName { get; set; }

		[DataMember(Name = "sn")]
		public string SenderName { get; set; }

		[DataMember(Name = "st")]
		public SnapStatus Status { get; set; }

		[DataMember(Name = "sts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime PostedAt { get; set; }

		[DataMember(Name = "ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Timestamp { get; set; }
		
		[IgnoreDataMember]
		public Boolean IsIncoming
		{
			get { return RecipientName == null; }
		}

		#region IComparable<Snap> Members

		public int CompareTo(Snap other)
		{
			return Convert.ToInt32((PostedAt - other.PostedAt).TotalSeconds);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return Convert.ToInt32((PostedAt - ((Snap)obj).PostedAt).TotalSeconds);
		}

		#endregion
	}
}
