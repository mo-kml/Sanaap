using Autofac;
using Bit;
using Bit.Model.Events;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
using Plugin.Geolocator;
using Prism;
using Prism.Autofac;
using Prism.Events;
using Prism.Ioc;
using Sanaap.App.ViewModels;
using Sanaap.App.Views;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Sanaap.App
{
    public partial class App : BitApplication
    {
        public App() : base(null)
        {
            MainPage = new NavigationPage(new MainView());
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
#if DEBUG
            LiveReload.Init();
#endif
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            bool isLoggedIn = await Container.Resolve<ISecurityService>().IsLoggedInAsync();

            if (isLoggedIn)
            {
                await NavigationService.NavigateAsync("Nav/Main");
            }
            else
            {
                await NavigationService.NavigateAsync("Register");
            }

            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<TokenExpiredEvent>()
                .Subscribe(async tokenExpiredEvent => await NavigationService.NavigateAsync("Login"), ThreadOption.UIThread);

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            ContainerBuilder containerBuilder = containerRegistry.GetBuilder();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>("Main");
            containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>("Register");
            containerRegistry.RegisterForNavigation<SubmitSosRequestView, SubmitSosRequestViewModel>("SubmitSosRequest");
            containerRegistry.RegisterForNavigation<MySosRequestsView, MySosRequestsViewModel>("MySosRequests");
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>("Menu");

            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                HostUri = new Uri("http://10.0.2.2:53148/"),       // Local
                //HostUri = new Uri("http://84.241.25.3:8220/"),   // Server

                // OAuthRedirectUri = new Uri("Test://oauth2redirect"),
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerRegistry.RegisterRequiredServices();
            containerRegistry.RegisterHttpClient();
            containerRegistry.RegisterODataClient();
            containerRegistry.RegisterIdentityClient();

            containerRegistry.Register<ICustomerValidator, DefaultCustomerValidator>();
            containerRegistry.Register<ILoginValidator, LoginValidator>();
            containerRegistry.RegisterSingleton<ITranslateService, DefaultTranslateService>();

            containerBuilder.Register(c => CrossGeolocator.Current).SingleInstance();

            base.RegisterTypes(containerRegistry);
        }
    }
}