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

            Select = new BitDelegateCommand(async () =>
              {
              });
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

            Contents = new ObservableCollection<Test>(

                new List<Test>
                {
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 157 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 1},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 137 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 0},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 155 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 1},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 457 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 1},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 127 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 0},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 156 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 0},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 257 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 1},
                    new Test{ Image = ImageSource.FromUri(new Uri("https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png")), Likes = 187 , Date = "1397/11/17" , Text = "متنی برای آزمایش میزان درستی قرار گیری اشیاء در کنار یکدیگر این یک متن آزمایشی است و این موضوع صرفا جهت طولانی تر شدن متن بوده و دلیل دیگری ندارد", Title = "عنوان آزمایشی برای برسی چگونگی قرارگیری اشیاء در کنار یکدیگر", YourLike = 1},
                });

            Image = "https://raw.githubusercontent.com/recurser/exif-orientation-examples/master/Landscape_3.jpg";
        }
        public ObservableCollection<Test> Contents { get; set; }


        public string Image { get; set; }

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
        public ImageSource Image { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Likes { get; set; }

        public int YourLike { get; set; }

        public string Date { get; set; }



    }


}
