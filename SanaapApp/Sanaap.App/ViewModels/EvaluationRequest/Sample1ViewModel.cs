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

            Comments.Add(new CommentsList
            {
                Code = 123456,
                StatusTypeName = "پاسخ داده شد",
                CreatedOn = DateTimeOffset.Now
            });

            Comments.Add(new CommentsList
            {
                Code = 123457,
                StatusTypeName = "در انتظار پاسخ",
                CreatedOn = DateTimeOffset.Now
            });

            Comments.Add(new CommentsList
            {
                Code = 123458,
                StatusTypeName = "پاسخ داده شد",
                CreatedOn = DateTimeOffset.Now
            });

            Comments.Add(new CommentsList
            {
                Code = 123459,
                StatusTypeName = "در انتظار پاسخ",
                CreatedOn = DateTimeOffset.Now
            });

        }

        public ObservableCollection<CommentsList> Comments { get; set; } = new ObservableCollection<CommentsList>();

    }

    public class CommentsList
    {
        public int Code { get; set; }
        public string StatusTypeName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
