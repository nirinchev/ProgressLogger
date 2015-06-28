using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using DryIoc;
using XLabs.Platform.Device;

namespace ProgressLogger.Core.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			Forms.Init();
			var app = new App(c =>
				{
					c.RegisterDelegate<IDevice>(_ => AppleDevice.CurrentDevice, Reuse.Singleton);
				});
			this.LoadApplication(app);

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}