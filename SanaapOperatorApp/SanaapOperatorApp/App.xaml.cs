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
using SanaapOperatorApp.MainModels;
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

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjI1MUAzMTM2MmUzMjJlMzBMc1YxeXdSMEJCd0pWcUI0STRtL2djTGFkQUhVUTdXVGtoQm0rRGZZUlRBPQ==;MjI1MkAzMTM2MmUzMjJlMzBHK1VXS3MrZFFqQmNFK3RtR0FaTGRCaWY2VHFRdGhZbDJCS3ZyZmRsUzBZPQ==;MjI1M0AzMTM2MmUzMjJlMzBrbU1DU3orbUJaeFY4Q1cybTFpU0dzTVZNSWF1dHd3OUVuRFA2VVN1SmFrPQ==;MjI1NEAzMTM2MmUzMjJlMzBkSUhlT1VHU3ZYMjhRSVdCcFFhY1dXdTVuYmMrN1ZBckY1SlZRVHAxc2VVPQ==;MjI1NUAzMTM2MmUzMjJlMzBvb2V4WHZ1bk40cjVmRmVKcnk1ZUp3MHFLVUJhK3FlczlteUpwUEh6YUxVPQ==;MjI1NkAzMTM2MmUzMjJlMzBjWWhadEF1eHFTQzd0RHU0ZVVQN1FoUlBRcWZTdm8zamtEVXZXVEZCQ2w0PQ==;MjI1N0AzMTM2MmUzMjJlMzBMTUxaNXdiYkJ6ejBEKzg5VlM4SzN5ZDNvUUV3VElaVXM2SnkvaFIvejA0PQ==;MjI1OEAzMTM2MmUzMjJlMzBrRlE3Ykp2dTBnWEpVTlZwYWJyQW9CYkExTUl3SVI3TGE0ZUFmNWxUaXlJPQ==;MjI1OUAzMTM2MmUzMjJlMzBjNFczQVBZTkFQQU80WFR2bTdhZXJuSmZheEl3KzRQWE55Rzg1cWFXb2hvPQ==");

            bool isLoggedIn = await Container.Resolve<ISecurityService>().IsLoggedInAsync();

            if (isLoggedIn)
            {
                await NavigationService.NavigateAsync("Menu/Nav/Main");
            }
            else
            {
                await NavigationService.NavigateAsync("/Register");
            }

            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<TokenExpiredEvent>()
                .SubscribeAsync(async tokenExpiredEvent => await NavigationService.NavigateAsync("Login"), ThreadOption.UIThread);

            await CrossMedia.Current.Initialize();

            await base.OnInitializedAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ContainerBuilder containerBuilder = containerRegistry.GetBuilder();

            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>("Menu");
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>("Main");
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
