﻿using Autofac;
using Bit;
using Bit.Model.Events;
using Bit.ViewModel.Contracts;
using Bit.ViewModel.Implementations;
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
        public App()
            : base(null)
        {

        }

        public App(IPlatformInitializer initializer)
                    : base(initializer)
        {

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("Nav/Main");

            //bool isLoggedIn = Container.Resolve<ISecurityService>().IsLoggedIn();

            //if (isLoggedIn)
            //{
            //    await NavigationService.NavigateAsync("Nav/Main");
            //}
            //else
            //{
            //    await NavigationService.NavigateAsync("Register");
            //}

            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<TokenExpiredEvent>()
                .Subscribe(async tokenExpiredEvent => await NavigationService.NavigateAsync("Login"), ThreadOption.UIThread);

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>("Main");
            containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>("Register");
            containerRegistry.RegisterForNavigation<ConfirmOtpView, ConfirmOtpViewModel>("ConfirmOtp");

            containerRegistry.GetBuilder().Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                HostUri = new Uri("http://192.168.1.51/Sanaap.Api/"),
                // OAuthRedirectUri = new Uri("Test://oauth2redirect"),
                AppName = "Sanaap",
                ODataRoute = "odata/Sanaap/"
            }).SingleInstance();

            containerRegistry.RegisterRequiredServices();
            containerRegistry.RegisterHttpClient();
            containerRegistry.RegisterODataClient();
            containerRegistry.RegisterIdentityClient();

            containerRegistry.Register<ICustomerValidator, DefaultCustomerValidator>();

            base.RegisterTypes(containerRegistry);
        }
    }
}
