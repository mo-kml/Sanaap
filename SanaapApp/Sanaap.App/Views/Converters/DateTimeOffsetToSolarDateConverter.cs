using Bit;
using Prism.Ioc;
using Sanaap.Service.Contracts;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanaap.App.Views.Converters
{
    public class DateTimeOffsetToSolarDateConverter : IValueConverter
    {
        private static readonly Lazy<IDateTimeUtils> DateTimeUtils = new Lazy<IDateTimeUtils>(((BitApplication)App.Current).Container.Resolve<IDateTimeUtils>);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
                return DateTimeUtils.Value.ConvertMiladiToShamsi(dateTimeOffset);

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
