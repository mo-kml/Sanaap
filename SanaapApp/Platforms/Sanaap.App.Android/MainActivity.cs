using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Bit;
using Bit.ViewModel;
using Bit.ViewModel.Implementations;
using FFImageLoading.Svg.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism.Ioc;
using Sanaap.App.Droid.Helpers;
using Sanaap.App.Helpers;
using Sanaap.App.ViewModels;
using System.Threading.Tasks;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Sanaap.App.Droid
{
    [Activity(Label = "Sanaap", Icon = "@drawable/launcher_foreground", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AppCenter.Start("7f0039a1-0052-4787-93af-36c5e1617617", typeof(Analytics), typeof(Crashes));

            BitExceptionHandler.Current = new SanaapExceptionHandler();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);
            SvgCachedImage.Init();
            UserDialogs.Init(this);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CrossCurrentActivity.Current.Init(this, bundle);

            FormsGoogleMaps.Init(this, bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            //BitCSharpClientControls.Init();

            //Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            Forms.Init(this, bundle);

            LoadApplication(new App(new SanaapAppDroidInitializer(this)));

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
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
            containerRegistry.Register<IAppUtilities, AndroidAppUtilities>();
            base.RegisterTypes(containerRegistry);
        }
    }

    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            Task.Run(() => StartActivity(new Intent(ApplicationContext, typeof(MainActivity))));
        }
    }
}
