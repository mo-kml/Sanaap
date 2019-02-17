using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Bit;
using Bit.Droid;
using Bit.ViewModel;
using Bit.ViewModel.Implementations;
using CarouselView.FormsPlugin.Android;
using ImageCircle.Forms.Plugin.Droid;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism.Ioc;
using Sanaap.App.Contracts;
using Sanaap.App.Droid.Helpers;
using Sanaap.App.Helpers;
using System.Threading.Tasks;
using Xamarin;
using Xamarin.Forms;

namespace Sanaap.App.Droid
{
    [Activity(Label = "Sanaap", Icon = "@drawable/launcher_foreground", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Orientation)]
    public class MainActivity : BitFormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AppCenter.Start("9c1f248c-5434-458f-9c4c-f69b39722c1f", typeof(Analytics), typeof(Crashes));

            BitExceptionHandler.Current = new SanaapExceptionHandler();

            UserDialogs.Init(this);

            base.OnCreate(bundle);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);

            UseDefaultConfiguration(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);

            CrossCurrentActivity.Current.Init(this, bundle);

            FormsGoogleMaps.Init(this, bundle);

            BitCSharpClientControls.Init();

            Forms.Init(this, bundle);

            CarouselViewRenderer.Init();

            ImageCircleRenderer.Init();

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            LoadApplication(new App(new SanaapAppDroidInitializer(this)));


            //Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
            //    .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
