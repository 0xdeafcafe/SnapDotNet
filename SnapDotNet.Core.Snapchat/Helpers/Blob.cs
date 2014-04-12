namespace SnapDotNet.Core.Snapchat.Helpers
{
	public static class Blob
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool ValidateMediaBlob(byte[] data)
		{
			if (data == null)
				return false;

			if (data.Length < 2)
				return false;

			return (data[0] == 0xFF && data[1] == 0xD8) ||
				   (data[0] == 0x00 && data[1] == 0x00);
		}
	}
}
