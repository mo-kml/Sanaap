using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Service.Contracts;
using System;
using System.Collections.Generic;

namespace SanaapOperatorApp.ViewModels
{
    public class LoginViewModel : BitViewModelBase
    {
        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual BitDelegateCommand Login { get; set; }

        public bool IsBusy { get; set; }

        public LoginViewModel(INavigationService navigationService,
            ISecurityService securityService,
            ISanaapOperatorAppLoginValidator loginValidator,
            IPageDialogService pageDialogService,
            ISanaapAppTranslateService translateService,
            IConnectivity connectivity)
        {
            Login = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
                    if (!loginValidator.IsValid(UserName, Password, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync("", translateService.Translate(errorMessage), "باشه");
                        return;
                    }

                    if (connectivity.IsConnected == false)
                    {
                        await pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                        return;
                    }

                    try
                    {
                        await securityService.LoginWithCredentials(UserName, Password, "SanaapOperatorAppResOwner", "secret");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string>
                        {
                            { "Message", ex.GetMessage() },
                            { "ViewModel", nameof(LoginViewModel) }
                        });
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
