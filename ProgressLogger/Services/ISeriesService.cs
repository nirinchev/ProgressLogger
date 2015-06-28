using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ProgressLogger.Services
{
	public interface ISeriesService
	{
		ObservableCollection<ISeriesInfo> CurrentSeries { get; }

		Task<IEnumerable<ISeriesInfo>> Find(string query);
	}
}