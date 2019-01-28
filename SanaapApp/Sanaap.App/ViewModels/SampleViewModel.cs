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
            Policies = new ObservableCollection<Test>(
                new List<Test>
                {
                    new Test{ CarName="پژو 405", ColorName="صولتی", InsuranceEndDate="1398/2/3", InsuranceTypeName="شخص ثالث", PlateNumber="https://setare.com/files/fa/news/1394/2/27/306_232.png",InsuranceImage="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"},
                    new Test{ CarName="پژو 405", ColorName="صولتی", InsuranceEndDate="1398/2/3", InsuranceTypeName="شخص ثالث", PlateNumber="https://setare.com/files/fa/news/1394/2/27/306_232.png",InsuranceImage="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"},
                    new Test{ CarName="پژو 405", ColorName="صولتی", InsuranceEndDate="1398/2/3", InsuranceTypeName="شخص ثالث", PlateNumber="https://setare.com/files/fa/news/1394/2/27/306_232.png",InsuranceImage="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"},
                }
                );


        }
        public ObservableCollection<Test> Policies { get; set; }


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
        public string InsuranceImage { get; set; }
        public string CarName { get; set; }
        public string ColorName { get; set; }
        public string InsuranceTypeName { get; set; }
        public string InsuranceEndDate { get; set; }
        public string PlateNumber { get; set; }
    }
}
