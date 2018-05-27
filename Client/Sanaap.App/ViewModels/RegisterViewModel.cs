using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Collections.Generic;

namespace Sanaap.App.ViewModels
{
    public class RegisterViewModel : BitViewModelBase
    {
        public virtual BitDelegateCommand Login { get; set; }

        public virtual BitDelegateCommand StartRegisteration { get; set; }

        public virtual CustomerDto Customer { get; set; } = new CustomerDto { };

        public bool IsBusy { get; set; }

        public RegisterViewModel(INavigationService navigationService,
            IODataClient oDataClient,
            ICustomerValidator customerValidator,
            IPageDialogService pageDialogService,
            ISecurityService securityService,
            ITranslateService translateService,
            IConnectivity connectivity)
        {
            Login = new BitDelegateCommand(async () =>
            {
                IsBusy = true;
                try
                {
                    await navigationService.NavigateAsync("Login");
                }
                finally
                {
                    IsBusy = false;
                }
            });

            StartRegisteration = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
                    if (connectivity.IsConnected == false)
                    {
                        await pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                        return;
                    }

                    if (!customerValidator.IsValid(Customer, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync("", translateService.Translate(errorMessage), "باشه");
                        return;
                    }

                    try
                    {
                        await oDataClient.For<CustomerDto>("Customers")
                            .Action("RegisterCustomer")
                            .Set(new
                            {
                                customer = Customer
                            })
                            .ExecuteAsync();

                        await securityService.LoginWithCredentials(Customer.NationalCode, Customer.Mobile, "SanaapResOwner", "secret");

                        await navigationService.NavigateAsync("Main");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string>
                        {
                            { "Message", ex.GetMessage() },
                            { "ViewModel", nameof(LoginViewModel) }
                        });

                        if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                        {
                            await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                        }
                        else
                        {
                            await pageDialogService.DisplayAlertAsync("خطای نامشخص", errorMessage, "باشه");
                        }
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
    }
}