using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using UIKit;
using System.Net;

namespace ProgressLogger.Helpers
{
	public static class HttpRequestHelper
	{
		private static readonly object lockObject = new object ();
		private static readonly LazyExecutor lazyIdlezor = new LazyExecutor (() =>
		{
			lock (lockObject)
			{
				if (loadingTasks != 0)
				{
					loadingTasks = 0;
					UpdateIdleStatusCore ();
				}
			}
		}, 10000);
		private static int loadingTasks = 0;

		public static event EventHandler OnUnauthorizedAccess;

		// TODO: consider making it task
		public static event EventHandler BecameIdle;

		public static bool IsIdle
		{
			get
			{
				return loadingTasks == 0;
			}
		}

		static HttpRequestHelper ()
		{
			#if DEBUG
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			#endif
		}

		public static Task<T> Get<T> (string endpoint, IDictionary<string, string> headers = null)
		{
			return Send<T> (HttpMethod.Get, endpoint, null, headers);
		}

		public static Task<HttpResponseMessage> Get (string endpoint, IDictionary<string, string> headers = null)
		{
			return Send (HttpMethod.Get, endpoint, null, headers);
		}

		public static Task<T> Post<T> (string endpoint, object payload = null, IDictionary<string, string> headers = null, Action<HttpClient> setupClient = null)
		{
			return Send<T> (HttpMethod.Post, endpoint, payload, headers, setupClient);
		}

		public static Task<T> Post<T> (string endpoint, HttpContent payload, IDictionary<string, string> headers = null, Action<HttpClient> setupClient = null)
		{
			return Send<T> (HttpMethod.Post, endpoint, payload, headers, setupClient);
		}

		public static Task<T> Patch<T> (string endpoint, IDictionary<string, object> payload = null, IDictionary<string, string> headers = null, Action<HttpClient> setupClient = null)
		{
			return Send<T> (new HttpMethod ("PATCH"), endpoint, payload, headers, setupClient);
		}

		public static async Task Delete (string endpoint, IDictionary<string, string> headers = null, Action<HttpClient> setupClient = null)
		{
			await Send (HttpMethod.Delete, endpoint, null, headers, setupClient);
		}

		public static Task<T> Send<T> (HttpMethod method, string endpoint, object payload, IDictionary<string, string> headers, Action<HttpClient> setupClient = null)
		{
			return Send<T> (method, endpoint, HttpContentHelper.GetJsonContent (payload), headers, setupClient);
		}

		public static async Task<T> Send<T> (HttpMethod method, string endpoint, HttpContent payload, IDictionary<string, string> headers, Action<HttpClient> setupClient = null)
		{
			var response = await Send (method, endpoint, payload, headers, setupClient);
			var content = await response.Content.ReadAsStringAsync ();
			try
			{
				return JsonConvert.DeserializeObject<T> (content);
			}
			catch
			{
				return default(T);
			}
		}

		private static async Task<HttpResponseMessage> Send (HttpMethod method, string endpoint, HttpContent content, IDictionary<string, string> headers, Action<HttpClient> setupClient = null)
		{
			try
			{
				using (var client = new HttpClient ())
				{
					if (setupClient != null)
					{
						setupClient (client);
					}

					var request = new HttpRequestMessage (method, endpoint);
					if (content != null && method != HttpMethod.Get)
					{
						request.Content = content;
					}

					if (headers != null)
					{
						foreach (var header in headers)
						{
							request.Headers.TryAddWithoutValidation (header.Key, header.Value);
						}
					}

					SetLoading (true);

					var response = await client.SendAsync (request);
					await CheckIsSuccess (response, string.Format ("Unable to {0} to {1}", method.Method, endpoint));
					return response;
				}
			}
			finally
			{
				SetLoading (false);
			}
		}

		private static async Task CheckIsSuccess (HttpResponseMessage response, string message)
		{
			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					RaiseUnauthorizedAccess ();
				}

				var content = await response.Content.ReadAsStringAsync ();

				throw new DetailedRequestException (response.StatusCode, string.Format ("{0}\nStatus code: {1}\nContent: {2}", message, response.StatusCode, content));
			}
		}

		public static void SetLoading (bool start)
		{
			lock (lockObject)
			{
				loadingTasks += start ? 1 : -1;
				UpdateIdleStatusCore ();
				lazyIdlezor.Run ();
			}
		}

		private static void UpdateIdleStatusCore ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = !IsIdle;

			if (IsIdle)
			{
				RaiseBecameIdle ();
			}
		}

		private static void RaiseUnauthorizedAccess ()
		{
			var handler = OnUnauthorizedAccess;
			if (handler != null)
			{
				handler (null, EventArgs.Empty);
			}
		}

		private static void RaiseBecameIdle ()
		{
			var handler = BecameIdle;
			if (handler != null)
			{
				handler (null, EventArgs.Empty);
			}
		}
	}
}