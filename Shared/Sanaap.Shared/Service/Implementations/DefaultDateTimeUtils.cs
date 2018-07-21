using Sanaap.Service.Contracts;
using System;
using System.Globalization;

namespace Sanaap.Service.Implementations
{
    public class DefaultDateTimeUtils : IDateTimeUtils
    {
        private readonly PersianCalendar persianCalendar = new PersianCalendar();

        public string ConvertMiladiToShamsi(DateTimeOffset dateTimeOffset)
        {
            string shamsiDate = persianCalendar.GetYear(dateTimeOffset.DateTime).ToString("0000/") + persianCalendar.GetMonth(dateTimeOffset.DateTime).ToString("00/") + persianCalendar.GetDayOfMonth(dateTimeOffset.DateTime).ToString("00");
            return shamsiDate;
        }

        public DateTimeOffset ConvertShamsiToMiladi(string shamsiDate)
        {
            string[] splitedDate = shamsiDate.Split('/');
            return persianCalendar.ToDateTime(int.Parse(splitedDate[0]), int.Parse(splitedDate[1]), int.Parse(splitedDate[2]), 0, 0, 0, 0);
        }

        public bool IsValidShamsiDate(string shamsiDate)
        {
            bool status = true;
            try
            {
                ConvertShamsiToMiladi(shamsiDate);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                status = false;
            }
            return status;
        }

    }
}
