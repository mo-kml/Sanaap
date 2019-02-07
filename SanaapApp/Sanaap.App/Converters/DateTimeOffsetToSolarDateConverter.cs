using Bit;
using Prism.Ioc;
using Sanaap.App.Helpers.Contracts;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanaap.App.Converters
{
    public class DateTimeOffsetToSolarDateConverter : IValueConverter
    {
        private static readonly Lazy<IDateHelper> DateHelper = new Lazy<IDateHelper>(BitApplication.Current.Container.Resolve<IDateHelper>);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return DateHelper.Value.ToPersianShortDate(dateTimeOffset.Date);
            }
            else if (value is DateTime dateTime)
            {
                return DateHelper.Value.ToPersianShortDate(dateTime);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
