using Acr.UserDialogs;
using Bit;
using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using System;

namespace Sanaap.App.ViewModels
{
    public class SanaapExceptionHandler : BitExceptionHandler
    {
        public async override void OnExceptionReceived(Exception exp)
        {
            Crashes.TrackError(exp);

            try
            {
                IUserDialogs userDialog = ((BitApplication)App.Current).Container.Resolve<IUserDialogs>();
                await userDialog.AlertAsync(message: exp.ToString(), title: exp.GetType().ToString());
            }
            catch (Exception ex)
            {

            }

            base.OnExceptionReceived(exp);
        }
    }
}
