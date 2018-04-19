using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;

namespace Sanaap.App.ViewModels
{
    public class RegisterViewModel : BitViewModelBase
    {
        public virtual BitDelegateCommand Login { get; set; }

        public virtual BitDelegateCommand ConfirmOtp { get; set; }

        public virtual CustomerDto Customer { get; set; } = new CustomerDto { };

        public RegisterViewModel(INavigationService navigationService,
            IODataClient oDataClient,
            ICustomerValidator customerValidator,
            IPageDialogService pageDialogService)
        {
            Login = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("Login");
            });

            ConfirmOtp = new BitDelegateCommand(async () =>
            {
                if (!customerValidator.IsValid(Customer, out string errorMessage))
                {
                    await pageDialogService.DisplayAlertAsync("/-:", errorMessage, ")-:");
                }

                int optCode = await oDataClient.For<CustomerDto>("Customers")
                    .Action("RegisterCustomer")
                    .Set(new
                    {
                        customer = Customer
                    })
                    .ExecuteAsScalarAsync<int>();

                await pageDialogService.DisplayAlertAsync("OTP", optCode.ToString(), "(-:");

                await navigationService.NavigateAsync("ConfirmOtp", new NavigationParameters
                {
                    { "nationalCode", Customer.NationalCode }
                });
            });
        }
    }
}
