using Sanaap.App.ItemSources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanaap.App.Converters
{
    public class LicensePlateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            LicensePlateItemSource licensePlate = (LicensePlateItemSource)value;

            return licensePlate.SecondNumber + " " + licensePlate.Alphabet + " " + licensePlate.FirstNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
