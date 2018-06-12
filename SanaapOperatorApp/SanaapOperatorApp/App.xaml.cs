using Autofac;
using Bit;
using Bit.Model.Events;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
using Prism;
using Prism.Autofac;
using Prism.Events;
using Prism.Ioc;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using SanaapOperatorApp.ViewModels;
using SanaapOperatorApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SanaapOperatorApp
{
    public partial class App : BitApplication
    {
        public App()
            : this(null)
        {

        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
#if DEBUG
            LiveReload.Init();
#endif
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();

            bool isLoggedIn = Container.Resolve<ISecurityService>().IsLoggedIn();

            if (isLoggedIn)
            {
                await NavigationService.NavigateAsync("Menu/Nav/Main");
            }
            else
            {
                await NavigationService.NavigateAsync("/Login");
            }

            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<TokenExpiredEvent>()
                .Subscribe(async tokenExpiredEvent => await NavigationService.NavigateAsync("Login"), ThreadOption.UIThread);

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ContainerBuilder containerBuilder = containerRegistry.GetBuilder();

            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>("Login");
            //containerRegistry.RegisterForNavigation<MainView, MainViewModel>("Main");
            //containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>("Register");
            //containerRegistry.RegisterForNavigation<SubmitSosRequestView, SubmitSosRequestViewModel>("SubmitSosRequest");
            //containerRegistry.RegisterForNavigation<MySosRequestsView, MySosRequestsViewModel>("MySosRequests");
            //containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>("Menu");

            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                HostUri = new Uri("http://localhost:53148/"),       // Local
                //HostUri = new Uri("http://84.241.25.3:8220/"),            // Server
                // OAuthRedirectUri = new Uri("Test://oauth2redirect"),
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerRegistry.RegisterRequiredServices();
            containerRegistry.RegisterHttpClient();
            containerRegistry.RegisterODataClient();
            containerRegistry.RegisterIdentityClient();

            containerRegistry.Register<ISanaapOperatorAppLoginValidator, SanaapOperatorAppLoginValidator>();
            containerRegistry.RegisterSingleton<ISanaapAppTranslateService, SanaapAppTranslateService>();

            base.RegisterTypes(containerRegistry);
        }
    }
}
