using Bit.ViewModel;
using Sanaap.App.Helpers.Contracts;
using System;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestDescriptionViewModel : BitViewModelBase
    {
        private readonly IDateHelper _dateHelper;
        public EvaluationRequestDescriptionViewModel(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
        }
        public string Month { get; set; }

        public string Year { get; set; }

        public string Day { get; set; }

        public DateTime? SelectedDate { get; set; }

        public void OnSelectedDateChanged()
        {
            _dateHelper.ToPersianLongDate(SelectedDate.Value, out string year, out string month, out string day);

            Year = year;
            Month = month;
            Day = day;
        }
    }
}
