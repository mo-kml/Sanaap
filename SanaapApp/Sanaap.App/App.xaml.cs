using Acr.UserDialogs;
using Autofac;
using Bit;
using Bit.Model.Events;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Media;
using Prism;
using Prism.Autofac;
using Prism.Events;
using Prism.Ioc;
using Sanaap.App.Controls;
using Sanaap.App.Controls.ViewModels;
using Sanaap.App.Events;
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
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Sanaap.App
{
    public partial class App : BitApplication
    {
        private IEventAggregator _eventAggregator;
        public App()
            : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
            InitializeComponent();

            OpenMenu = new BitDelegateCommand<IconButton>(async (button) =>
            {
                AbsoluteLayout menu = ((AbsoluteLayout)button.BindingContext);

                ToggleMenu(menu);
            });
        }

        protected override async Task OnInitializedAsync()
        {
            InitializeComponent();

            CultureInfo.CurrentUICulture = new CultureInfo("en");

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDM0QDMxMzYyZTMzMmUzMENvVC95ZVRKQUVLdjJMMjJ6MGNXOFBkanNnR3hIVkpDTnJIVkh6b3VQWXc9;NDM1QDMxMzYyZTMzMmUzMFdaTUFWU21TK3hZV3MzUUNxWWZJTWtPRmkxZGllakNndE5ZSDIxRUhEWHM9;NDM2QDMxMzYyZTMzMmUzMGZLMmJzTFhoQWJRS081Mk56VTFISlQxOThMVzZiS3J2WTdsR0sveU1CcG89;NDM3QDMxMzYyZTMzMmUzMEhpS2gza1NzNlhrMWtjS0tZanRpWG10NzEwcjYwb1hqUnNoOEZqWmZCa2M9;NDM4QDMxMzYyZTMzMmUzMFhpUUtnY1NXMFN5b1o4Nmd3cVpTUXA1NzdGZTQ1bmM5bElQL0NLY1Q1b0U9;NDM5QDMxMzYyZTMzMmUzMEJFcUduNHd3aG9tQUNLdnhEWUpycW5abUtpWmhJZVlkMFk3QkZKZjJVUXc9;NDQwQDMxMzYyZTMzMmUzMFNHM2Z2RmphRGZWTUtGRThDTWFaNmNlU0p6aFpRaC9OQmhUWkxNQ1RRZFE9;NDQxQDMxMzYyZTMzMmUzMFZ6WFUwWm81ZDMvTG8weDVybFBUQ1U0S1J5M2tHekxqaVdUTnVFTWJoVjg9;NDQyQDMxMzYyZTMzMmUzMERlVlkvRjZ2WnlkcmFuTm5SQUdPQ2tVVHNYY1Jla0UyeVVSZFdIaGRaVlk9;");

            bool isLoggedIn = await Container.Resolve<ISecurityService>().IsLoggedInAsync();

            if (isLoggedIn)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMenuView)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(LoginView)}");
            }

            //await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SampleView)}");


            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            _eventAggregator = eventAggregator;

            eventAggregator.GetEvent<TokenExpiredEvent>()
                .SubscribeAsync(async tokenExpiredEvent =>
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginView)}"), ThreadOption.UIThread);

            eventAggregator.GetEvent<ToggleMenuEvent>()
                .SubscribeAsync(async menu => ToggleMenu(menu), ThreadOption.UIThread, true);

            await CrossMedia.Current.Initialize();

            await base.OnInitializedAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry, ContainerBuilder containerBuilder, IServiceCollection services)
        {
            containerRegistry.RegisterForNav<NavigationPage>();
            containerRegistry.RegisterForNav<SampleView, SampleViewModel>();
            containerRegistry.RegisterForNav<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNav<ContactUsView, ContactUsViewModel>();
            containerRegistry.RegisterForNav<ContentListView, ContentListViewModel>();
            //
            containerRegistry.RegisterForNav<ShowContentView, ShowContentViewModel>();
            containerRegistry.RegisterForNav<MainMenuView, MainMenuViewModel>();
            containerRegistry.RegisterForNav<RegisterView, RegisterViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestMapView, EvaluationRequestMapViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestExpertView, EvaluationRequestExpertViewModel>();
            containerRegistry.RegisterForNav<OpenImagePopup, OpenImagePopupViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestWaitView, EvaluationRequestWaitViewModel>();
            containerRegistry.RegisterForNav<CommentListView, CommentListViewModel>();
            containerRegistry.RegisterForNav<CommentAnswerPopupView, CommentAnswerPopupViewModel>();
            containerRegistry.RegisterForNav<CreateInsurancePolicyView, CreateInsurancePolicyViewModel>();
            containerRegistry.RegisterForNav<InsurancePolicyListView, InsurancePolicyListViewModel>();
            containerRegistry.RegisterForNav<EvlRequestProgressView, EvlRequestProgressViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestListView, EvaluationRequestListViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestMenuView, EvaluationRequestMenuViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestDetailView, EvaluationRequestDetailViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestFilesView, EvaluationRequestFilesViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestLostDetailView, EvaluationRequestLostDetailViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestDescriptionView, EvaluationRequestDescriptionViewModel>();
            containerRegistry.RegisterForNav<EvaluationRequestExpertRankView, EvaluationRequestExpertRankViewModel>();

            containerRegistry.RegisterPartialView<MenuView, MenuViewModel>();
            containerRegistry.RegisterPartialView<InsuranceListPopupView, InsuranceListPopupViewModel>();


            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
#if DEBUG
                HostUri = new Uri("http://192.168.143.2:53148/"),
#elif !DEBUG
                HostUri = new Uri("http://sanapapi.ipinsur.com/"),
#endif
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerBuilder.RegisterRequiredServices();
            containerBuilder.RegisterODataClient();
            containerBuilder.RegisterHttpClient();
            containerBuilder.RegisterIdentityClient();


            containerRegistry.Register<ICustomerValidator, DefaultCustomerValidator>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<ICommentValidator, DefaultCommentValidator>();
            containerRegistry.Register<IInsuranceValidator, DefaultInsuranceValidator>();
            containerRegistry.Register<IEvlRequestValidator, DefaultEvlRequestValidator>();
            containerRegistry.Register<INewsService, NewsService>();
            containerRegistry.Register<IPolicyService, PolicyService>();
            containerRegistry.Register<IDateHelper, DateHelper>();
            containerRegistry.Register<ILicenseHelper, LicenseHelper>();
            containerRegistry.Register<ICommentService, CommentService>();
            containerRegistry.Register<IEvlRequestService, EvlRequestService>();
            containerRegistry.Register<ISanaapAppLoginValidator, SanaapAppLoginValidator>();
            containerRegistry.RegisterSingleton<IInitialDataService, InitialDataService>();
            containerRegistry.RegisterSingleton<IDateTimeUtils, DefaultDateTimeUtils>();
            containerRegistry.RegisterSingleton<ISanaapAppTranslateService, SanaapAppTranslateService>();


            containerRegistry.RegisterInstance(CrossMedia.Current);
            containerRegistry.RegisterInstance(UserDialogs.Instance);

            containerRegistry.RegisterSingleton<IDateTimeUtils, DefaultDateTimeUtils>();

            base.RegisterTypes(containerRegistry, containerBuilder, services);
        }

        public void ToggleMenu(AbsoluteLayout menu)
        {
            if (menu.TranslationX == 0)
            {
                menu.FindByName<Button>("menuButton").IsVisible = false;

                menu.TranslateTo(DeviceDisplay.MainDisplayInfo.Width, 0, 350);
            }
            else
            {
                menu.TranslateTo(0, 0, 350);

                menu.FindByName<Button>("menuButton").IsVisible = true;
            }
        }

        public void ToggleMenuButton(object sender, EventArgs e)
        {
            AbsoluteLayout menu;

            if (sender is IconButton iconButton)
            {
                menu = ((AbsoluteLayout)((IconButton)sender).BindingContext);
            }
            else
            {
                menu = ((AbsoluteLayout)((Button)sender).Parent);
            }

            ToggleMenu(menu);
        }

        public BitDelegateCommand<IconButton> OpenMenu { get; set; }
    }
}
