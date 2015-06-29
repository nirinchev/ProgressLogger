using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using DryIoc.MefAttributedModel;
using ProgressLogger.RemoteClients.TMDb;
using System.Collections.ObjectModel;
using ProgressLogger.Models;

namespace ProgressLogger.Services.Implementation
{
	[Export(typeof(ISeriesService)), SingletonReuseAttribute]
	public class SeriesService : ISeriesService
	{
		private const string CachedSeriesKey = "CachedSeries";
		private readonly TMDbClient client;
		private readonly ICacheService cacheService;

		public ObservableCollection<SeriesInfo> CurrentSeries { get; } = new ObservableCollection<SeriesInfo>();

		[ImportingConstructor]
		public SeriesService(TMDbClient client, ICacheService cacheService)
		{
			this.client = client;
			this.cacheService = cacheService;
			this.CurrentSeries.CollectionChanged += (sender, e) => this.cacheService.AddOrUpdate(CachedSeriesKey, this.CurrentSeries.ToArray());

			this.Initialize().Forget();
		}

		private async Task Initialize()
		{
			var cachedInfoes = await this.cacheService.GetValueOrDefault<SeriesInfo[]>(CachedSeriesKey);
			this.CurrentSeries.AddRange(cachedInfoes);
		}

		public Task<IEnumerable<SeriesInfo>> Find(string query)
		{
			return this.client.Search(query);
		}
	}
}