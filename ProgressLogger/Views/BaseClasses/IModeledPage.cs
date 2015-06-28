using GalaSoft.MvvmLight;

namespace ProgressLogger.Views.BaseClasses
{
	public interface IModeledPage<T> where T : ViewModelBase
	{
		T ViewModel { get; }
	}
}