using DryIoc.MefAttributedModel;
using System.ComponentModel.Composition;
using ProgressLogger.RemoteClients.TMDb.Models;
using System.Threading.Tasks;
using ProgressLogger.Helpers;
using ProgressLogger.Core;
using System.Collections.Generic;

namespace ProgressLogger.RemoteClients.TMDb
{
	[Export, SingletonReuseAttribute]
	public class TMDbClient
	{
		private const string BaseUrl = "http://api.themoviedb.org/3/";
		private readonly TaskCompletionSource<Configuration> configurationTcs;

		public TMDbClient()
		{
			this.configurationTcs = new TaskCompletionSource<Configuration>();
			this.Initialize().Forget();
		}

		public async Task<IEnumerable<ISeriesInfo>> Search(string query)
		{
			var endpoint = GetEndpoint("search/tv", $"query={query}");
			var response = await HttpRequestHelper.Get<TMDbCollection<TMDbSeriesInfo>>(endpoint);
			var config = await configurationTcs.Task;
			foreach (var info in response.Items)
			{
				info.PosterUrl = config.ImagesConfiguration.GetPosterPath(info.PosterPath, 200);
			}

			return response.Items;
		}

		private async Task Initialize()
		{
			var endpoint = GetEndpoint("configuration");
			var config = await HttpRequestHelper.Get<Configuration>(endpoint);
			this.configurationTcs.SetResult(config);
		}

		private static string GetEndpoint(string component, string query = null)
		{
			const string apiKey = ApplicationConstants.TMDbApiKey;
			query = string.IsNullOrEmpty(query) ? string.Empty : $"&{query}";
			return $"{BaseUrl}{component}?api_key={apiKey}{query}";
		}
	}
}