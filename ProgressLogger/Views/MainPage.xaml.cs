using System.ComponentModel.Composition;
using Xamarin.Forms;
using ProgressLogger.ViewModels;

namespace ProgressLogger.Views
{
	[Export]
	public partial class MainPage : ContentPage
	{
		private readonly MainViewModel viewModel;

		[ImportingConstructor]
		public MainPage(MainViewModel viewModel)
		{
			this.viewModel = viewModel;
			
			this.InitializeComponent();
			this.BindingContext = viewModel;
			NavigationPage.SetBackButtonTitle(this, string.Empty);
		}

		private void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				if (viewModel.ShowDetailsCommand.CanExecute(e.SelectedItem))
				{
					viewModel.ShowDetailsCommand.Execute(e.SelectedItem);
				}

				((ListView)sender).SelectedItem = null;
			}
		}
	}
}