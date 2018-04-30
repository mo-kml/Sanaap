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

        public virtual BitDelegateCommand StartRegisteration { get; set; }

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

            StartRegisteration = new BitDelegateCommand(async () =>
            {
                if (!customerValidator.IsValid(Customer, out string errorMessage))
                {
                    await pageDialogService.DisplayAlertAsync("اشکال در ثبت اطلاعات", errorMessage, "باشه");
                    return;
                }

                await oDataClient.For<CustomerDto>("Customers")
                    .Action("RegisterCustomer")
                    .Set(new
                    {
                        customer = Customer
                    })
                    .ExecuteAsync();
            });
        }
    }
}
