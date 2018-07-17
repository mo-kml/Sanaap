using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Threading;

namespace Sanaap.App.ViewModels
{
    public class RegisterViewModel : BitViewModelBase
    {
        public virtual BitDelegateCommand Login { get; set; }

        public virtual BitDelegateCommand Register { get; set; }

        public virtual CustomerDto Customer { get; set; } = new CustomerDto { };

        private CancellationTokenSource registerCancellationTokenSource;

        public RegisterViewModel(INavigationService navigationService,
            IODataClient oDataClient,
            ICustomerValidator customerValidator,
            IPageDialogService pageDialogService,
            ISecurityService securityService,
            ISanaapAppTranslateService translateService,
            IUserDialogs userDialogs)
        {
            Login = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("Login");
            });

            Register = new BitDelegateCommand(async () =>
            {
                registerCancellationTokenSource?.Cancel();
                registerCancellationTokenSource = new CancellationTokenSource();

                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
                {
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
                            .ExecuteAsync(registerCancellationTokenSource.Token);

                        await securityService.LoginWithCredentials(Customer.NationalCode, Customer.Mobile, "SanaapResOwner", "secret", cancellationToken: registerCancellationTokenSource.Token);

                        await navigationService.NavigateAsync("/Menu/Nav/Main");
                    }
                    catch (Exception ex)
                    {
                        if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                            await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                        else
                            throw;
                    }
                }
            });
        }
    }
}
