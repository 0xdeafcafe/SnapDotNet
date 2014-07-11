using System;
using System.Net.Http;

namespace Snapchat.SnapLogic.Api.Exceptions
{
	public class InvalidHttpResponseException : Exception
	{
		public HttpResponseMessage HttpResponseMessage { get; private set; }

		public InvalidHttpResponseException() { }

		public InvalidHttpResponseException(string message, HttpResponseMessage httpResponseMessage)
			: base(message)
		{
			HttpResponseMessage = httpResponseMessage;
		}
	}
}
