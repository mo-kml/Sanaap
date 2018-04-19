using Android.App;
using Android.Content.PM;
using Android.OS;
using Bit.ViewModel.Implementations;
using Prism.Ioc;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Sanaap.App.Droid
{
    [Activity(Label = "Sanaap.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FormsMaps.Init(this, bundle);

            Forms.Init(this, bundle);

            LoadApplication(new App(new SanaapAppDroidInitializer(this)));
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

