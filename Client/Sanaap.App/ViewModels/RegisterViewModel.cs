using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;

namespace Sanaap.App.ViewModels
{
    public class RegisterViewModel : BitViewModelBase
    {
        public virtual BitDelegateCommand Login { get; set; }

        public virtual BitDelegateCommand StartRegisteration { get; set; }

        public virtual CustomerDto Customer { get; set; } = new CustomerDto { };

        public RegisterViewModel(INavigationService navigationService,
            IODataClient oDataClient,
            ICustomerValidator customerValidator,
            IPageDialogService pageDialogService,
            ISecurityService securityService)
        {
            Login = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("Login");
            });

            StartRegisteration = new BitDelegateCommand(async () =>
            {
                if (!customerValidator.IsValid(Customer, out string errorMessage))
                {
                    await pageDialogService.DisplayAlertAsync("اشکال در ثبت اطلاعات", errorMessage, "باشه");
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
                catch(Exception ex)
                {
                    await pageDialogService.DisplayAlertAsync("قبلا ثبت نام شده اید", errorMessage, "باشه");
                    throw;
                }
            });
        }
    }
}