using Plugin.Media.Abstractions;
using System;
using System.Globalization;
using System.IO;

namespace Sanaap.App.Helpers
{
    public class Helpers
    {
        public static string ConvertStreamToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static string ConvertMediaFileToBase64(MediaFile stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.GetStream().CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static string ConvertDateToShamsi(DateTimeOffset dateTimeOffset)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            string solarDate = persianCalendar.GetYear(dateTimeOffset.DateTime).ToString("0000/") + persianCalendar.GetMonth(dateTimeOffset.DateTime).ToString("00/") + persianCalendar.GetDayOfMonth(dateTimeOffset.DateTime).ToString("00");

            return solarDate;
        }

        public static DateTime ConvertShamsiToMiladi(string date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            string[] splitedDate = date.Split('/');

            return persianCalendar.ToDateTime(int.Parse(splitedDate[0]), int.Parse(splitedDate[1]), int.Parse(splitedDate[2]), 0, 0, 0, 0);
        }

        public static bool IsShamsiDateValid(string date)
        {
            DateTime dateTime = DateTime.Now;

            return DateTime.TryParse(date, new CultureInfo("fa-IR"), DateTimeStyles.None, out dateTime);
        }
    }
}
