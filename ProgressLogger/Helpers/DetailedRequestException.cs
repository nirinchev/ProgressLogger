using System;
using System.Net;

namespace ProgressLogger.Helpers
{
	public class DetailedRequestException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }

		public DetailedRequestException (HttpStatusCode code, string message) : base (message)
		{
			this.StatusCode = code;
		}
	}
}