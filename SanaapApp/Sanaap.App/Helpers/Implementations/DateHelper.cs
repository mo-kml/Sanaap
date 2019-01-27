using NodaTime;
using Sanaap.App.Helpers.Contracts;
using System;
using System.Globalization;

namespace Sanaap.App.Helpers.Implementations
{
    public class DateHelper : IDateHelper
    {
        public void ToPersianLongDate(DateTime dateTime, out string year, out string month, out string day)
        {
            LocalDate date = toPersian(dateTime, out CultureInfo cultureInfo);

            day = date.ToString("dd", cultureInfo);
            month = date.ToString("MMM", cultureInfo);
            year = date.ToString("yyyy", cultureInfo);
        }

        public string ToPersianShortDate(DateTime dateTime)
        {
            LocalDate date = toPersian(dateTime, out CultureInfo cultureInfo);

            return date.ToString("yyyy/MM/dd", cultureInfo);
        }

        private LocalDate toPersian(DateTime dateTime, out CultureInfo cultureInfo)
        {
            cultureInfo = new CultureInfo("Fa");

            cultureInfo.DateTimeFormat.MonthNames = cultureInfo.DateTimeFormat.AbbreviatedMonthGenitiveNames = cultureInfo.DateTimeFormat.MonthGenitiveNames = cultureInfo.DateTimeFormat.AbbreviatedMonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };

            return new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day, CalendarSystem.Gregorian)
                  .WithCalendar(CalendarSystem.PersianArithmetic);
        }
    }
}
