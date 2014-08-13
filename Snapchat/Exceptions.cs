using System;
using Windows.Web.Http;

namespace SnapDotNet
{
	/// <summary>
	/// The exception that is thrown when a set of credentials is invalid.
	/// </summary>
	public class InvalidCredentialsException
		: Exception
	{
		public InvalidCredentialsException() { }
		public InvalidCredentialsException(string message)
			: base(message)
		{
		}
	}

	/// <summary>
	/// The exception that is thrown when a HTTP response is invalid or unexpected.
	/// </summary>
	public class InvalidHttpResponseException
		: Exception
	{
		public InvalidHttpResponseException() { }
		public InvalidHttpResponseException(string message, HttpResponseMessage httpResponseMessage)
			: base(message)
		{
			HttpResponseMessage = httpResponseMessage;
		}

		public HttpResponseMessage HttpResponseMessage { get; private set; }
	}
}
