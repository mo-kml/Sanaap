using Acr.UserDialogs;
using Autofac;
using Bit;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
using Plugin.Media;
using Prism;
using Prism.Autofac;
using Prism.Events;
using Prism.Ioc;
using Sanaap.App.Controls;
using Sanaap.App.Controls.ViewModels;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.Helpers.Implementations;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Services.Implementations;
using Sanaap.App.ViewModels;
using Sanaap.App.ViewModels.Comment;
using Sanaap.App.ViewModels.Content;
using Sanaap.App.ViewModels.EvaluationRequest;
using Sanaap.App.ViewModels.Insurance;
using Sanaap.App.Views;
using Sanaap.App.Views.Comment;
using Sanaap.App.Views.Content;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.App.Views.Insurance;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using Syncfusion.SfNavigationDrawer.XForms;
using System;
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

            InitializeComponent();

            LiveReload.Init();
#endif
        }

        protected override async Task OnInitializedAsync()
        {
            InitializeComponent();



            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjI1MUAzMTM2MmUzMjJlMzBMc1YxeXdSMEJCd0pWcUI0STRtL2djTGFkQUhVUTdXVGtoQm0rRGZZUlRBPQ==;MjI1MkAzMTM2MmUzMjJlMzBHK1VXS3MrZFFqQmNFK3RtR0FaTGRCaWY2VHFRdGhZbDJCS3ZyZmRsUzBZPQ==;MjI1M0AzMTM2MmUzMjJlMzBrbU1DU3orbUJaeFY4Q1cybTFpU0dzTVZNSWF1dHd3OUVuRFA2VVN1SmFrPQ==;MjI1NEAzMTM2MmUzMjJlMzBkSUhlT1VHU3ZYMjhRSVdCcFFhY1dXdTVuYmMrN1ZBckY1SlZRVHAxc2VVPQ==;MjI1NUAzMTM2MmUzMjJlMzBvb2V4WHZ1bk40cjVmRmVKcnk1ZUp3MHFLVUJhK3FlczlteUpwUEh6YUxVPQ==;MjI1NkAzMTM2MmUzMjJlMzBjWWhadEF1eHFTQzd0RHU0ZVVQN1FoUlBRcWZTdm8zamtEVXZXVEZCQ2w0PQ==;MjI1N0AzMTM2MmUzMjJlMzBMTUxaNXdiYkJ6ejBEKzg5VlM4SzN5ZDNvUUV3VElaVXM2SnkvaFIvejA0PQ==;MjI1OEAzMTM2MmUzMjJlMzBrRlE3Ykp2dTBnWEpVTlZwYWJyQW9CYkExTUl3SVI3TGE0ZUFmNWxUaXlJPQ==;NTQ5MzJAMzEzNjJlMzQyZTMwRUhkejlFelR5REwzVVNTRC8zaGtOWEZJNWxxRUJxTW9ZbWRUelNNWEJoTT0=");

            bool isLoggedIn = await Container.Resolve<ISecurityService>().IsLoggedInAsync();

            //if (isLoggedIn)
            //{
            //    await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMenuView)}");
            //}
            //else
            //{
            //    await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(LoginView)}");
            //}
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(EvaluationRequestExpertView)}");


            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();


            //eventAggregator.GetEvent<TokenExpiredEvent>()
            //    .SubscribeAsync(async tokenExpiredEvent => await NavigationService.NavigateAsync(nameof(LoginView)), ThreadOption.UIThread);

            await CrossMedia.Current.Initialize();

            await base.OnInitializedAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ContainerBuilder containerBuilder = containerRegistry.GetBuilder();

            containerRegistry.RegisterForNav<NavigationPage>();
            containerRegistry.RegisterForNav<SampleView, SampleViewModel>();
            containerRegistry.RegisterForNav<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNav<ContactUsView, ContactUsViewModel>();
            containerRegistry.RegisterForNav<ContentListView, ContentListViewModel>();
            containerRegistry.RegisterForNav<ShowContentView, ShowContentViewModel>();
            containerRegistry.RegisterForNav<MainMenuView, MainMenuViewModel>();
            containerRegistry.RegisterForNav<RegisterView, RegisterViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestMapView, EvaluationRequestMapViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestExpertView, EvaluationRequestExpertViewModel>();
            containerRegistry.RegisterForNav<OpenImagePopup, OpenImagePopupViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestWaitView, EvaluationRequestWaitViewModel>();
            containerRegistry.RegisterForNav<CommentListView, CommentListViewModel>();
            containerRegistry.RegisterForNav<CreateInsurancePolicyView, CreateInsurancePolicyViewModel>();
            containerRegistry.RegisterForNav<InsurancePolicyListView, InsurancePolicyListViewModel>();
            //
            containerRegistry.RegisterForNav<EvlRequestProgressView, EvlRequestProgressViewModel>();
            //
            containerRegistry.RegisterForNav<EvaluationRequestListView, EvaluationRequestListViewModel>();
            containerRegistry.RegisterForNav<EvlRequestInquiryView, EvlRequestInquiryViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestView, EvaluationRequestViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestMenuView, EvaluationRequestMenuViewModel>();

            containerRegistry.RegisterForNav<EvaluationRequestDetailView, EvaluationRequestDetailViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestFilesView, EvaluationRequestFilesViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestLostDetailView, EvaluationRequestLostDetailViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestDescriptionView, EvaluationRequestDescriptionViewModel>();


            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                //HostUri = new Uri("http://84.241.25.3:8220/"),         // Server
                HostUri = new Uri("http://192.168.143.2:53148/"),
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerBuilder.RegisterRequiredServices();
            containerBuilder.RegisterODataClient();
            containerBuilder.RegisterHttpClient();
            containerBuilder.RegisterIdentityClient();


            containerRegistry.Register<ICustomerValidator, DefaultCustomerValidator>();
            containerRegistry.Register<ICommentValidator, DefaultCommentValidator>();
            containerRegistry.Register<IInsuranceValidator, DefaultInsuranceValidator>();
            containerRegistry.Register<IEvlRequestValidator, DefaultEvlRequestValidator>();
            containerRegistry.Register<INewsService, NewsService>();
            containerRegistry.Register<IPolicyService, PolicyService>();
            containerRegistry.Register<IDateHelper, DateHelper>();
            containerRegistry.Register<ICommentService, CommentService>();
            containerRegistry.Register<IEvlRequestService, EvlRequestService>();
            containerRegistry.Register<ISanaapAppLoginValidator, SanaapAppLoginValidator>();
            containerRegistry.RegisterSingleton<IInitialDataService, InitialDataService>();
            containerRegistry.RegisterSingleton<IDateTimeUtils, DefaultDateTimeUtils>();
            containerRegistry.RegisterSingleton<ISanaapAppTranslateService, SanaapAppTranslateService>();

            containerBuilder.Register(c => new MenuViewModel(Container.Resolve<ISecurityService>(), NavigationService));
            containerBuilder.Register(c => new InsuranceListPopupViewModel(Container.Resolve<IEventAggregator>(), Container.Resolve<IPolicyService>(), Container.Resolve<IUserDialogs>(), NavigationService));
            containerBuilder.Register(c => CrossMedia.Current).SingleInstance();
            containerBuilder.Register(c => UserDialogs.Instance).SingleInstance();

            containerRegistry.RegisterSingleton<IDateTimeUtils, DefaultDateTimeUtils>();

            base.RegisterTypes(containerRegistry);
        }

        public void ToggleMenu(object sender, EventArgs e)
        {
            ((SfNavigationDrawer)((IconButton)sender).BindingContext).ToggleDrawer();
        }
    }
}
