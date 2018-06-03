using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanaap.App.Views.Converters
{
    public class DateTimeOffsetToSolarDateConverter : IValueConverter
    {
        private readonly PersianCalendar persianCalendar = new PersianCalendar();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                string solarDate = persianCalendar.GetYear(dateTimeOffset.DateTime).ToString("0000/") + persianCalendar.GetMonth(dateTimeOffset.DateTime).ToString("00/") + persianCalendar.GetDayOfMonth(dateTimeOffset.DateTime).ToString("00");

                if (parameter is string stringFormat)
                    return string.Format(stringFormat, solarDate);
                else
                    return solarDate;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
