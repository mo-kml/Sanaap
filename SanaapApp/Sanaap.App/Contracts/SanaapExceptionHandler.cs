using Acr.UserDialogs;
using Bit;
using Bit.ViewModel;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using Sanaap.Constants;
using System;
using System.Collections.Generic;

namespace Sanaap.App.Contracts
{
    public class SanaapExceptionHandler : BitExceptionHandler
    {
        public override async void OnExceptionReceived(Exception exp, IDictionary<string, string> properties = null)
        {
            Crashes.TrackError(exp, properties);
            Analytics.TrackEvent(exp.Message, properties);

            try
            {
                IUserDialogs userDialog = BitApplication.Current.Container.Resolve<IUserDialogs>();
                await userDialog.AlertAsync(message: ConstantStrings.UnknownError, title: ConstantStrings.Error);

                throw exp;
            }
            catch (Exception ex)
            {

            }

            base.OnExceptionReceived(exp);
        }
    }
}
