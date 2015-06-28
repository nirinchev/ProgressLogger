using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ProgressLogger.Models;

namespace ProgressLogger.Services
{
	public interface ISeriesService
	{
		ObservableCollection<SeriesInfo> CurrentSeries { get; }

		Task<IEnumerable<SeriesInfo>> Find(string query);
	}
}