using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;

namespace Sanaap.App.ViewModels
{
    public class LoginViewModel : BitViewModelBase
    {
        public virtual LoginDto loginDto { get; set; } = new LoginDto { };

        public virtual BitDelegateCommand StartLogin { get; set; }

        public LoginViewModel(INavigationService navigationService, IODataClient oDataClient, ILoginValidator loginValidator, IPageDialogService pageDialogService)
        {
            StartLogin = new BitDelegateCommand(async () =>
            {
                if (!loginValidator.IsValid(loginDto, out string errorMessage))
                {
                    await pageDialogService.DisplayAlertAsync("اشکال در ثبت اطلاعات", errorMessage, "باشه");
                    return;
                }

                try
                {
                    await oDataClient.For<LoginDto>("Customers")
                        .Action("Login")
                        .Set(new
                        {
                            loginDto = loginDto
                        })
                        .ExecuteAsync();

                    await navigationService.NavigateAsync("Main");
                }
                catch (Exception ex)
                {
                    await pageDialogService.DisplayAlertAsync(ex.Message, errorMessage, "باشه");
                    var a = ex.Message;
                }
            });
        }
    }
}