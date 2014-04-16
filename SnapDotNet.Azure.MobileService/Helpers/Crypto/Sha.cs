using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SnapDotNet.Azure.MobileService.Helpers.Crypto
{
	public class Sha
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Sha256(string data)
		{
			var crypt = new SHA256Managed();
			var hash = String.Empty;
			var crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));
			return crypto.Aggregate(hash, (current, bit) => current + bit.ToString("x2"));
		}
	}
}
