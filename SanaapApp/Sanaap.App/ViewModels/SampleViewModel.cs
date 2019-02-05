using Bit.ViewModel;
using PropertyChanged;
using Sanaap.App.Helpers.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class SampleViewModel : BitViewModelBase
    {
        private readonly IDateHelper _dateHelper;
        public SampleViewModel(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
            //Contents = new ObservableCollection<Test>(
            //    new List<Test>
            //    {
            //        new Test{ Image=ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Title="مغفول ماندن بیمه رانندگان طی چند سال اخیر", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=125,YourLike=1,Date="1397/10/23"},
            //        new Test{ Image=ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Title="مغفول ماندن بیمه رانندگان طی چند سال اخیر", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=225,YourLike=0,Date="1397/10/23"},
            //        new Test{ Image=ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Title="مغفول ماندن بیمه رانندگان طی چند سال اخیر", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=345,YourLike=0,Date="1397/10/23"},
            //        new Test{ Image=ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Title="مغفول ماندن بیمه رانندگان طی چند سال اخیر", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=120,YourLike=1,Date="1397/10/23"},
            //        new Test{ Image=ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Title="مغفول ماندن بیمه رانندگان طی چند سال اخیر", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=54,YourLike=0 ,Date="1397/10/23"}
            //    }
            //    );

            //Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")),

            Policies = new ObservableCollection<Test>(

                new List<Test>
                {
                    new Test{LastNumber = 11}
                });


        }
        public ObservableCollection<Test> Policies { get; set; }

        


        //public Test Content { get; set; }

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
    [AddINotifyPropertyChangedInterface]
    public class Test
    {
        public int LastNumber { get; set; }

    }

    
}
