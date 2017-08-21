
using Foundation;
using UIKit;

namespace Transport.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsGoogleMaps.Init("AIzaSyDPHKQm19PW2cwtfyBuPbLl0-1LEIlCM7w");
            //AIzaSyB4NkBRISt78_mirRUotrxdNk5tMXdTFtA
            LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
