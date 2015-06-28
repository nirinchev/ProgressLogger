using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProgressLogger
{
	public interface INavigationService
	{
		void Initialize(NavigationPage navPage);

		Task NavigateTo(string key, bool modal);
	}
}