using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using Sanaap.Service.Contracts;
using System;
using System.Threading;

namespace Sanaap.App.ViewModels
{
    public class LoginViewModel : BitViewModelBase
    {
        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual BitDelegateCommand Login { get; set; }

        private CancellationTokenSource registerCancellationTokenSource;

        public LoginViewModel(INavigationService navigationService,
            ISecurityService securityService,
            ISanaapAppLoginValidator loginValidator,
            IPageDialogService pageDialogService,
            ISanaapAppTranslateService translateService,
            IUserDialogs userDialogs)
        {
            Login = new BitDelegateCommand(async () =>
            {
                registerCancellationTokenSource?.Cancel();
                registerCancellationTokenSource = new CancellationTokenSource();
                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
                {
                    if (!loginValidator.IsValid(NationalCode, Mobile, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync("", translateService.Translate(errorMessage), "باشه");
                        return;
                    }

                    try
                    {
                        await securityService.LoginWithCredentials(NationalCode, Mobile, "SanaapResOwner", "secret");
                        await navigationService.NavigateAsync("/Menu/Nav/Main");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("CustomerCouldNotBeFound"))
                            await pageDialogService.DisplayAlertAsync("", "کاربری با این مشخصات یافت نشد", "باشه");
                        else if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                            await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                        else
                            throw;
                    }
                }
            });
        }
    }
}
