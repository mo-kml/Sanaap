using Sanaap.Service.Contracts;
using System;
using System.Globalization;

namespace Sanaap.Service.Implementations
{
    public class DefaultDateTimeUtils : IDateTimeUtils
    {
        public string ConvertDateToShamsi(DateTimeOffset dateTimeOffset)
        {
            string shamsiDate = persianCalendar.GetYear(dateTimeOffset.DateTime).ToString("0000/") + persianCalendar.GetMonth(dateTimeOffset.DateTime).ToString("00/") + persianCalendar.GetDayOfMonth(dateTimeOffset.DateTime).ToString("00");

            return shamsiDate;
        }

        public DateTimeOffset ConvertShamsiToMiladi(string shamsiDate)
        {
            string[] splitedDate = shamsiDate.Split('/');

            return persianCalendar.ToDateTime(int.Parse(splitedDate[0]), int.Parse(splitedDate[1]), int.Parse(splitedDate[2]), 0, 0, 0, 0);
        }

        public bool IsValidShamsiDate(string date)
        {
            return DateTimeOffset.TryParse(date, faIrCultureInfo, DateTimeStyles.None, out _);
        }

        private readonly CultureInfo faIrCultureInfo = new CultureInfo("fa-IR");
        private readonly PersianCalendar persianCalendar = new PersianCalendar();
    }
}
