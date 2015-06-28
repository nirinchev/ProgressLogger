using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using GalaSoft.MvvmLight;

namespace ProgressLogger.Services
{
	public interface INavigationService
	{
		void Initialize(NavigationPage navPage);

		Task NavigateTo<T>(Action<T> onNavigated = null) where T : ViewModelBase;
	}
}