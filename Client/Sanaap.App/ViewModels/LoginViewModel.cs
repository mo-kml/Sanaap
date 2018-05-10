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
                    await pageDialogService.DisplayAlertAsync("اشکال در ثبت اطلاعات", errorMessage, "باشه");
                    return;
                }

                try
                {
                    await securityService.LoginWithCredentials(NationalCode, Mobile, "SanaapResOwner", "secret");

                    await navigationService.NavigateAsync("Main");
                }
                catch (Exception ex)
                {
                    await pageDialogService.DisplayAlertAsync(ex.Message, errorMessage, "باشه");
                    throw;
                }
            });
        }
    }
}