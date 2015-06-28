using GalaSoft.MvvmLight;
using System.ComponentModel.Composition;

namespace ProgressLogger
{
	[Export]
	public class MainViewModel : ViewModelBase
	{
		public string Title 
		{
			get
			{
				return "Main page";
			}
		}
	}
}