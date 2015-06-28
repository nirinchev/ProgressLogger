using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using DryIoc.MefAttributedModel;
using ProgressLogger.RemoteClients.TMDb;
using System.Collections.ObjectModel;

namespace ProgressLogger.Services.Implementation
{
	[Export(typeof(ISeriesService)), SingletonReuseAttribute]
	public class SeriesService : ISeriesService
	{
		private const string CachedSeriesKey = "CachedSeries";
		private readonly TMDbClient client;
		private readonly ICacheService cacheService;

		public ObservableCollection<ISeriesInfo> CurrentSeries { get; } = new ObservableCollection<ISeriesInfo>();

		[ImportingConstructor]
		public SeriesService(TMDbClient client, ICacheService cacheService)
		{
			this.client = client;
			this.cacheService = cacheService;
		}

		private async Task Initialize()
		{
			var cachedInfoes = await this.cacheService.GetValueOrDefault<IEnumerable<ISeriesInfo>>(CachedSeriesKey);
			this.CurrentSeries.AddRange(cachedInfoes);
		}

		public Task<IEnumerable<ISeriesInfo>> Find(string query)
		{
			return this.client.Search(query);
		}
	}
}