using System;

namespace Sanaap.App.Helpers.Contracts
{
    public interface IDateHelper
    {
        void ToPersianLongDate(DateTime dateTime, out string year, out string month, out string day);
        string ToPersianShortDate(DateTime dateTime);
    }
}
