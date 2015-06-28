using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ProgressLogger.Helpers
{
	public static class HttpContentHelper
	{
		public static HttpContent GetJsonContent (object payload)
		{
			return HttpContentHelper.GetJsonContent (JsonConvert.SerializeObject (payload));
		}

		public static HttpContent GetJsonContent (string payload)
		{
			return new StringContent (payload, Encoding.UTF8, "application/json");
		}
	}
}