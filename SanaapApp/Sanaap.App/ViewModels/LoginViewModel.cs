using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Service.Contracts;
using System;
using System.Collections.Generic;

namespace Sanaap.App.ViewModels
{
    public class LoginViewModel : BitViewModelBase
    {
        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual BitDelegateCommand Login { get; set; }

        public bool IsBusy { get; set; }

        public LoginViewModel(INavigationService navigationService,
            ISecurityService securityService,
            ISanaapAppLoginValidator loginValidator,
            IPageDialogService pageDialogService,
            ISanaapAppTranslateService translateService,
            IConnectivity connectivity)
        {
            Login = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
                    if (!loginValidator.IsValid(NationalCode, Mobile, out string errorMessage))
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
                        await securityService.LoginWithCredentials(NationalCode, Mobile, "SanaapResOwner", "secret");
                        await navigationService.NavigateAsync("/Menu/Nav/Main");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string>
                        {
                            { "Message", ex.GetMessage() },
                            { "ViewModel", nameof(LoginViewModel) }
                        });

                        if (ex.Message.Contains("CustomerCouldNotBeFound"))
                        {
                            await pageDialogService.DisplayAlertAsync("", "کاربری با این مشخصات یافت نشد", "باشه");
                            return;
                        }

                        if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                            await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                        else
                            await pageDialogService.DisplayAlertAsync("خطای نامشخص", errorMessage, "باشه");
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
