using GalaSoft.MvvmLight;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Linq;
using ProgressLogger.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProgressLogger.Services;
using System.Collections.Specialized;
using System;
using ProgressLogger.Models;

namespace ProgressLogger.ViewModels
{
	[Export]
	public class MainViewModel : ViewModelBase, IDisposable
	{
		private readonly ISeriesService seriesService;
		private readonly INavigationService navigationService;
		private readonly LazyExecutor remoteSearchLazy;

		private string searchQuery;

		public string Title 
		{
			get
			{
				return "Tracked series";
			}
		}

		public string SearchQuery
		{
			get
			{
				return this.searchQuery;
			}
			set
			{
				if (this.Set(ref this.searchQuery, value, true))
				{
					this.Search();
				}
			}
		}

		public ObservableCollection<Grouping<string, SeriesInfo>> Series { get; }

		public ICommand SearchCommand { get; }

		public RelayCommand<SeriesInfo> ShowDetailsCommand { get; }

		[ImportingConstructor]
		public MainViewModel(ISeriesService seriesService, INavigationService navigationService)
		{
			this.remoteSearchLazy = new LazyExecutor(this.SearchRemote);

			this.seriesService = seriesService;
			this.navigationService = navigationService;

			this.Series = new ObservableCollection<Grouping<string, SeriesInfo>>();
			this.Series.Add(new Grouping<string, SeriesInfo>("Tracking"));

			this.SearchCommand = new RelayCommand(this.Search);
			this.ShowDetailsCommand = new RelayCommand<SeriesInfo>(this.NavigateToSeriesDetails, info => info != null);

			this.seriesService.CurrentSeries.CollectionChanged += this.OnCurrentSeriesChanged;
			this.Search();
		}

		public void Dispose()
		{
			this.seriesService.CurrentSeries.CollectionChanged -= this.OnCurrentSeriesChanged;
		}

		private void OnCurrentSeriesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			this.Search();
		}

		private void Search()
		{
			this.SearchLocal();

			if (string.IsNullOrEmpty(this.SearchQuery))
			{
				this.RemoveRemoteGroup();
			}
			else
			{
				this.remoteSearchLazy.Run();
			}
		}

		private void SearchLocal()
		{
			var local = this.Series[0];
			var toRemove = local.Where(i => !i.Filter(this.SearchQuery) || !this.seriesService.CurrentSeries.Contains(i));
			local.RemoveRange(toRemove);

			var toAdd = this.seriesService.CurrentSeries.Where(i => !local.Contains(i) && i.Filter(this.SearchQuery));
			local.AddRange(toAdd);

			local.Sort(i => i.Name);
		}

		private async Task SearchRemote()
		{
			var items = await this.seriesService.Find(this.SearchQuery);
			this.AddRemoteGroup(items.Where(i => this.seriesService.CurrentSeries.All(c => c.Id != i.Id)));
		}

		private void RemoveRemoteGroup()
		{
			var remote = this.Series.ElementAtOrDefault(1);
			if (remote != null)
			{
				this.Series.Remove(remote);
			}
		}

		private async void NavigateToSeriesDetails(SeriesInfo info)
		{
			await this.navigationService.NavigateTo<DetailsViewModel>(vm => vm.Info = info);
		}

		private void AddRemoteGroup(IEnumerable<SeriesInfo> infoes)
		{
			this.RemoveRemoteGroup();
			var remote = new Grouping<string, SeriesInfo>("On the web", infoes);
			this.Series.Add(remote);
		}
	}
}