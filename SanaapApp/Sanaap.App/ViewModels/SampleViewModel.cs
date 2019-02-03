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
            Comments = new ObservableCollection<Test>(
                new List<Test>
                {
                    new Test{ Code=1235673, StatusTypeName="پاسخ داده شده", CreatedOn=DateTimeOffset.Now},
                    new Test{ Code=1235673, StatusTypeName="در حال برسی", CreatedOn=DateTimeOffset.Now},
                    new Test{ Code=1235673, StatusTypeName="پاسخ داده شده", CreatedOn=DateTimeOffset.Now},
                    new Test{ Code=1235673, StatusTypeName="در حال برسی", CreatedOn=DateTimeOffset.Now},
                    new Test{ Code=1235673, StatusTypeName="پاسخ داده شده", CreatedOn=DateTimeOffset.Now}
                }
                );


        }
        public ObservableCollection<Test> Comments { get; set; }


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
        public int Code { get; set; }
        public string StatusTypeName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        
    }
}
