using System;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace SnapDotnet.Miscellaneous.Crypto
{
	public class Sha
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Sha256(string data)
		{
			var bytes = Encoding.UTF8.GetBytes(data);
			var digest = new Sha256Digest();
			for (var i = 0; i < digest.GetDigestSize(); ++i)
				digest.Update((byte) i);
			digest.BlockUpdate(bytes, 0, bytes.Length);
			return DigestUtilities.DoFinal(digest).Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));
		}
	}
}
