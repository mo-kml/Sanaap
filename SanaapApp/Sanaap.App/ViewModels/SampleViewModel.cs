using Bit.ViewModel;
using Sanaap.App.Helpers.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sanaap.App.ViewModels
{
    public class SampleViewModel : BitViewModelBase
    {
        private readonly IDateHelper _dateHelper;
        public SampleViewModel(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
            Requests = new ObservableCollection<Test>(
                new List<Test>
                {
                    new Test{ RequestId=1235673, RequestTypeName="خودرو", Date="1398/2/3"},
                    new Test{ RequestId=1235673, RequestTypeName="خودرو", Date="1398/2/3"},
                    new Test{ RequestId=1235673, RequestTypeName="خودرو", Date="1398/2/3"},
                    new Test{ RequestId=1235673, RequestTypeName="خودرو", Date="1398/2/3"},
                    new Test{ RequestId=1235673, RequestTypeName="خودرو", Date="1398/2/3"}
                }
                );


        }
        public ObservableCollection<Test> Requests { get; set; }


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

        public BitDelegateCommand Select { get; set; }
    }
    public class Test
    {
        public int RequestId { get; set; }
        public string RequestTypeName { get; set; }
        public string Date { get; set; }
        
    }
}
