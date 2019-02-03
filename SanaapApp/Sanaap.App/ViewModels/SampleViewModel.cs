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
            //        new Test{ Image="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png", Title="عنوان 1", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=125,YourLike=1,Date="1397/10/23"},
            //        new Test{ Image="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png", Title="عنوان 2", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=225,YourLike=0,Date="1397/10/23"},
            //        new Test{ Image="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png", Title="عنوان 3", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=345,YourLike=0,Date="1397/10/23"},
            //        new Test{ Image="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png", Title="عنوان 4", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=120,YourLike=1,Date="1397/10/23"},
            //        new Test{ Image="https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png", Title="عنوان 5", Text="مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",Likes=54,YourLike=0 ,Date="1397/10/23"}
            //    }
            //    );

            Content = new Test() {
            Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")),
            Title = "خروج از برجام گزینه کنونی تهران است",
            Text = "مدیرعامل اتحادیه تاکسیرانی های شهری  کشور گفت : متاسفانه موضوع بسیار مهم تامین اجتماعی رانندگان طی چند سال اخیر مغفول مانده، در صورتی ",
            Date = "1397/10/23",
            YourLike = true,
            Likes = 154,
            };




        }
        //public ObservableCollection<Test> Contents { get; set; }

        public Test Content { get; set; }

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
        public ImageSource Image { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public bool YourLike { get; set; }
        public string Date { get; set; }

    }
}
