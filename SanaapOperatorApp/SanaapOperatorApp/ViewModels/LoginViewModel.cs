using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Microsoft.AppCenter.Crashes;
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

        public LoginViewModel(ISecurityService securityService)
        {
            Login = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
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
