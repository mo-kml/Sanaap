using Bit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class Sample1ViewModel : BitViewModelBase
    {
        public Sample1ViewModel()
        {
            Contents.Add(new CommentsList
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"
                ,
                Title = "عنوان شماره 1"
                ,
                IsLiked = true
                ,
                LikeCount = 124
                ,
                CreatedDate = "1395/2/20"
                ,MainText = "بی نظمی بیشتر از هر ویژگی منفرد دیگری استرس آفرین است. اما مهارت نظم بخشی می تواند آموخته شود. از اینجا شروع کنید که درباره آنچه میخواهید در طی زمان معین انجام دهید، فکر کرده و تعیین نمایید که این تکلیف را چگونه انجام خواهید داد که بیشترین کارایی را داشته باشد. زمان و میزان فعالیت هایی که باید در آن چهرچوب های زمانی انجام شود را یادداشت کنید و طبق همان پیش بروید مثلا یک روز خاص هفته به خرید اختصاص یابد و برسی کنید که آیا این کار باعث کاهش استرس میشود؟"
            });

            Contents.Add(new CommentsList
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"
                ,
                Title = "عنوان شماره 2"
                ,
                IsLiked = false
                ,
                LikeCount = 122
                ,
                CreatedDate = "1396/5/12"
                ,
                MainText = "بی نظمی بیشتر از هر ویژگی منفرد دیگری استرس آفرین است. اما مهارت نظم بخشی می تواند آموخته شود. از اینجا شروع کنید که درباره آنچه میخواهید در طی زمان معین انجام دهید، فکر کرده و تعیین نمایید که این تکلیف را چگونه انجام خواهید داد که بیشترین کارایی را داشته باشد. زمان و میزان فعالیت هایی که باید در آن چهرچوب های زمانی انجام شود را یادداشت کنید و طبق همان پیش بروید مثلا یک روز خاص هفته به خرید اختصاص یابد و برسی کنید که آیا این کار باعث کاهش استرس میشود؟"
            });

            Contents.Add(new CommentsList
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"
                ,
                Title = "عنوان شماره 3"
                ,
                IsLiked = true
                ,
                LikeCount = 32
                ,
                CreatedDate = "1395/6/25"
                ,
                MainText = "بی نظمی بیشتر از هر ویژگی منفرد دیگری استرس آفرین است. اما مهارت نظم بخشی می تواند آموخته شود. از اینجا شروع کنید که درباره آنچه میخواهید در طی زمان معین انجام دهید، فکر کرده و تعیین نمایید که این تکلیف را چگونه انجام خواهید داد که بیشترین کارایی را داشته باشد. زمان و میزان فعالیت هایی که باید در آن چهرچوب های زمانی انجام شود را یادداشت کنید و طبق همان پیش بروید مثلا یک روز خاص هفته به خرید اختصاص یابد و برسی کنید که آیا این کار باعث کاهش استرس میشود؟"
            });

        }

        public ObservableCollection<CommentsList> Contents { get; set; } = new ObservableCollection<CommentsList>();
        public long RequestCode { get; set; }

    }

    public class CommentsList
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public bool IsLiked { get; set; }
        public int LikeCount { get; set; }
        public string CreatedDate { get; set; }
        public string MainText { get; set; }
    }
}
