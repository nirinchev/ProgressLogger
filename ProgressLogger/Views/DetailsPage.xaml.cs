using Xamarin.Forms;
using System.ComponentModel.Composition;
using ProgressLogger.ViewModels;
using ProgressLogger.Views.BaseClasses;

namespace ProgressLogger.Views
{
	[Export]
	public partial class DetailsPage : ContentPage, IModeledPage<DetailsViewModel>
	{
		public DetailsViewModel ViewModel { get; }

		[ImportingConstructor]
		public DetailsPage(DetailsViewModel viewModel)
		{
			this.ViewModel = viewModel;

			this.InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}