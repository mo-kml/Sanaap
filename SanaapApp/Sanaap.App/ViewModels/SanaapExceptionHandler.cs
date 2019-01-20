using Acr.UserDialogs;
using Bit;
using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using System;
using System.Collections.Generic;

namespace Sanaap.App.ViewModels
{
    public class SanaapExceptionHandler : BitExceptionHandler
    {
        public override async void OnExceptionReceived(Exception exp, IDictionary<string, string> properties = null)
        {
            Crashes.TrackError(exp, properties);

            try
            {
                IUserDialogs userDialog = BitApplication.Current.Container.Resolve<IUserDialogs>();
                await userDialog.AlertAsync(message: exp.ToString(), title: exp.GetType().ToString());

                //throw exp;
            }
            catch (Exception ex)
            {

            }

            base.OnExceptionReceived(exp);
        }
    }
}
