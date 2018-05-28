using Android.App;
using Android.Content.PM;
using Android.OS;
using Bit.ViewModel.Implementations;
using FFImageLoading.Forms.Droid;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism.Ioc;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Sanaap.App.Droid
{
    [Activity(Label = "Sanaap", Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AppCenter.Start("7f0039a1-0052-4787-93af-36c5e1617617", typeof(Analytics), typeof(Crashes));

            CachedImageRenderer.Init(true);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CrossCurrentActivity.Current.Init(this, bundle);

            FormsGoogleMaps.Init(this, bundle);

            Forms.Init(this, bundle);

            LoadApplication(new App(new SanaapAppDroidInitializer(this)));

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class SanaapAppDroidInitializer : BitPlatformInitializer
    {
        public SanaapAppDroidInitializer(Activity activity)
            : base(activity)
        {

        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
        }
    }
}