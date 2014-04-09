using System;
using System.Net.Http;

namespace SnapDotNet.Core.Snapchat.Api.Exceptions
{
	public class InvalidHttpResponseException : Exception
	{
		public HttpResponseMessage HttpResponseMessage { get; private set; }

		public InvalidHttpResponseException(string message, HttpResponseMessage httpResponseMessage)
			: base(message)
		{
			HttpResponseMessage = httpResponseMessage;
		}
	}
}
