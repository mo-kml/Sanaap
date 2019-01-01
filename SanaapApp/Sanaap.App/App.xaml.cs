using Acr.UserDialogs;
using Autofac;
using Bit;
using Bit.Model.Events;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
using Plugin.Media;
using Prism;
using Prism.Autofac;
using Prism.Events;
using Prism.Ioc;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Services.Implementations;
using Sanaap.App.ViewModels;
using Sanaap.App.ViewModels.Comment;
using Sanaap.App.ViewModels.Content;
using Sanaap.App.ViewModels.Insurance;
using Sanaap.App.Views;
using Sanaap.App.Views.Comment;
using Sanaap.App.Views.Content;
using Sanaap.App.Views.Insurance;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Sanaap.App
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

        protected override async Task OnInitializedAsync()
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
                await NavigationService.NavigateAsync("/Login");
            }

            //await NavigationService.NavigateAsync("CreatePolicy");

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
            containerRegistry.RegisterForNavigation<ContactUsView, ContactUsViewModel>("ContactUs");
            containerRegistry.RegisterForNavigation<ContentListView, ContentListViewModel>("ContentList");
            containerRegistry.RegisterForNavigation<ShowContentView, ShowContentViewModel>("ShowContent");
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>("Main");
            containerRegistry.RegisterForNavigation<MainInsuranceView, MainInsuranceViewModel>("MainInsurance");
            containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>("Register");
            containerRegistry.RegisterForNavigation<SubmitSosRequestView, SubmitSosRequestViewModel>("SubmitSosRequest");
            containerRegistry.RegisterForNavigation<SosRequestView, SosRequestViewModel>("SosRequest");
            containerRegistry.RegisterForNavigation<MySosRequestsView, MySosRequestsViewModel>("MySosRequests");
            containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>("Menu");
            containerRegistry.RegisterForNavigation<EvlRequestMapView, EvlRequestMapViewModel>("EvlRequestMap");
            containerRegistry.RegisterForNavigation<EvlRequestDetailView, EvlRequestDetailViewModel>("EvlRequestDetail");
            containerRegistry.RegisterForNavigation<EvlRequestFilesView, EvlRequestFilesViewModel>("EvlRequestFiles");
            containerRegistry.RegisterForNavigation<EvlRequestWaitView, EvlRequestWaitViewModel>("EvlRequestWait");
            containerRegistry.RegisterForNavigation<CreateCommentView, CreateCommentViewModel>("CreateComment");
            containerRegistry.RegisterForNavigation<CommentListView, CommentListViewModel>("CommentList");
            containerRegistry.RegisterForNavigation<CreateInsurancePolicyView, CreateInsurancePolicyViewModel>("CreatePolicy");
            containerRegistry.RegisterForNavigation<InsurancePolicyListView, InsurancePolicyListViewModel>("InsurancePolicyList");

            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                //HostUri = new Uri("http://10.0.2.2:53148/"),            // Emulator
                //HostUri = new Uri("http://192.168.10.112:53148/"),       // Device : ip Iranian Pooshesh
                //HostUri = new Uri("http://192.168.1.207:53148/"),       // Device : ip Moradi
                //HostUri = new Uri("http://84.241.25.3:8220/"),         // Server
                HostUri = new Uri("http://d82169e9.ngrok.io/"),         // Kamali

                //OAuthRedirectUri = new Uri("Test://oauth2redirect"),
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerBuilder.Register(c => new HttpClient
            {
                BaseAddress = new Uri("http://5.144.128.234:8800/api/Portal/"),
            }).SingleInstance();

            containerRegistry.RegisterRequiredServices();
            containerRegistry.RegisterODataClient();
            containerRegistry.RegisterHttpClient();
            containerRegistry.RegisterIdentityClient();

            containerRegistry.Register<ICustomerValidator, DefaultCustomerValidator>();
            containerRegistry.Register<ICommentValidator, DefaultCommentValidator>();
            containerRegistry.Register<IPolicyService, PolicyService>();
            containerRegistry.Register<IInsurerService, InsurerService>();
            containerRegistry.Register<ISanaapAppLoginValidator, SanaapAppLoginValidator>();
            containerRegistry.RegisterSingleton<IInitialDataService, InitialDataService>();
            containerRegistry.RegisterSingleton<ISanaapAppTranslateService, SanaapAppTranslateService>();

            containerBuilder.Register(c => CrossMedia.Current).SingleInstance();
            containerBuilder.Register(c => UserDialogs.Instance).SingleInstance();

            containerRegistry.RegisterSingleton<IDateTimeUtils, DefaultDateTimeUtils>();

            base.RegisterTypes(containerRegistry);
        }
    }
}
