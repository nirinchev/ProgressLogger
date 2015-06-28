using GalaSoft.MvvmLight;
using System.ComponentModel.Composition;
using ProgressLogger.RemoteClients.TMDb;
using System.Threading.Tasks;
using AutoMapper;
using ProgressLogger.Models;

namespace ProgressLogger.ViewModels
{
	[Export]
	public class DetailsViewModel : ViewModelBase
	{
		private readonly TMDbClient client;

		private SeriesInfo info;

		public SeriesInfo Info
		{
			get
			{
				return this.info;
			}
			set
			{
				this.Set(ref this.info, value, true);

				if (!this.info.IsComplete())
				{
					this.Reload().Forget();
				}
			}
		}

		public DetailsViewModel(TMDbClient client)
		{
			this.client = client;
		}

		private async Task Reload()
		{
			var details = await this.client.LoadDetails(this.Info.Id);
			Mapper.Map(details, this.info);
			this.RaisePropertyChanged(nameof(Info));
		}
	}
}