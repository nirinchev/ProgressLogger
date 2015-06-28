using System.ComponentModel.Composition;
using Xamarin.Forms;

namespace ProgressLogger
{
	[Export]
	public partial class MainPage : ContentPage
	{
		[ImportingConstructor]
		public MainPage(MainViewModel viewModel)
		{
			this.InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}