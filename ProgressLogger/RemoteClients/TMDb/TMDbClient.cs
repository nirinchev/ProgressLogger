using DryIoc.MefAttributedModel;
using System.ComponentModel.Composition;
using ProgressLogger.RemoteClients.TMDb.Models;
using System.Threading.Tasks;
using ProgressLogger.Helpers;
using ProgressLogger.Core;
using System.Collections.Generic;
using XLabs.Platform.Device;
using ProgressLogger.Models;
using AutoMapper;
using System.Linq;

namespace ProgressLogger.RemoteClients.TMDb
{
	[Export, SingletonReuseAttribute]
	public class TMDbClient
	{
		private const string BaseUrl = "http://api.themoviedb.org/3/";
		private readonly TaskCompletionSource<Configuration> configurationTcs;
		private readonly IDevice device;

		[ImportingConstructor]
		public TMDbClient(IDevice device)
		{
			this.device = device;
			this.configurationTcs = new TaskCompletionSource<Configuration>();
			this.Initialize().Forget();
		}

		public async Task<IEnumerable<SeriesInfo>> Search(string query)
		{
			var endpoint = GetEndpoint("search/tv", $"query={query}");
			var response = await HttpRequestHelper.Get<TMDbCollection<TMDbSeriesInfo>>(endpoint);
			var config = await configurationTcs.Task;
			foreach (var info in response.Items)
			{
				config.ImagesConfiguration.UpdateUrls(info, 200, this.device.Display.Width);
			}

			return response.Items.Select(Mapper.Map<SeriesInfo>);
		}

		public async Task<SeriesInfo> LoadDetails(int id)
		{
			var endpoint = GetEndpoint($"tv/{id}");
			var response = await HttpRequestHelper.Get<TMDbSeriesInfo>(endpoint);
			var config = await configurationTcs.Task;
			config.ImagesConfiguration.UpdateUrls(response, 200, this.device.Display.Width);
			return Mapper.Map<SeriesInfo>(response);
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