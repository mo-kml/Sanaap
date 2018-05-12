using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Service.Contracts;
using System;

namespace Sanaap.App.ViewModels
{
    public class LoginViewModel : BitViewModelBase
    {
        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual BitDelegateCommand StartLogin { get; set; }

        public LoginViewModel(INavigationService navigationService,
            ISecurityService securityService,
            ILoginValidator loginValidator,
            IPageDialogService pageDialogService)
        {
            StartLogin = new BitDelegateCommand(async () =>
            {
                if (!loginValidator.IsValid(NationalCode, Mobile, out string errorMessage))
                {
                    await pageDialogService.DisplayAlertAsync("", errorMessage, "باشه");
                    return;
                }

                try
                {
                    await securityService.LoginWithCredentials(NationalCode, Mobile, "SanaapResOwner", "secret");

                    await navigationService.NavigateAsync("/Main");
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("invalid_grant"))
                        await pageDialogService.DisplayAlertAsync("", "کاربری یافت نشد", "باشه");
                    else
                    await pageDialogService.DisplayAlertAsync(ex.Message, errorMessage, "باشه");
                    throw;
                }
            });
        }
    }
}