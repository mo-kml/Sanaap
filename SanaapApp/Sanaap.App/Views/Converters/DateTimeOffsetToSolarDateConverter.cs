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
                return Helpers.Helpers.ConvertDateToShamsi(dateTimeOffset);
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
