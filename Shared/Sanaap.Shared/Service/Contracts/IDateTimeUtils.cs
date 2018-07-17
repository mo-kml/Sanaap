using System;

namespace Sanaap.Service.Contracts
{
    public interface IDateTimeUtils
    {
        string ConvertDateToShamsi(DateTimeOffset dateTimeOffset);

        DateTimeOffset ConvertShamsiToMiladi(string shamsiDate);

        bool IsValidShamsiDate(string date);
    }
}
