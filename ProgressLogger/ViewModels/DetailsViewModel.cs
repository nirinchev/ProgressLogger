using GalaSoft.MvvmLight;
using System.ComponentModel.Composition;
using ProgressLogger.RemoteClients.TMDb;
using System.Threading.Tasks;
using AutoMapper;
using ProgressLogger.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ProgressLogger.Services;
using System.Linq;

namespace ProgressLogger.ViewModels
{
	[Export]
	public class DetailsViewModel : ViewModelBase
	{
		private readonly TMDbClient client;
		private readonly ISeriesService seriesService;

		private SeriesInfo info;

		public SeriesInfo Info
		{
			get
			{
				return this.info;
			}
			set
			{
				if (this.Set(ref this.info, value, true))
				{
					this.RaisePropertyChanged(nameof(IsTracking));
				}

				if (!this.info.IsComplete())
				{
					this.Reload().Forget();
				}
			}
		}

		public ICommand StartTrackingCommand { get; }

		public bool IsTracking
		{
			get
			{
				return this.Info != null && this.seriesService.CurrentSeries.Any(s => s.Id == this.Info.Id);
			}
		}

		[ImportingConstructor]
		public DetailsViewModel(TMDbClient client, ISeriesService seriesService)
		{
			this.client = client;
			this.seriesService = seriesService;

			this.StartTrackingCommand = new RelayCommand(this.StartTracking);
		}

		private async Task Reload()
		{
			var details = await this.client.LoadDetails(this.Info.Id);
			Mapper.Map(details, this.Info);

			this.RaisePropertyChanged(nameof(Info));
		}

		private void StartTracking()
		{
			var existing = this.seriesService.CurrentSeries.FirstOrDefault(s => s.Id == this.Info.Id);
			if (existing == null)
			{
				this.seriesService.CurrentSeries.Add(this.Info);
			}
			else
			{
				this.seriesService.CurrentSeries.Remove(existing);
			}

			this.RaisePropertyChanged(nameof(IsTracking));
		}
	}
}