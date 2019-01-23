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

            Progresses.Add(new CommentsList
            {
                Status = "Test1",
                Date="1398/5/6"
            });

            Progresses.Add(new CommentsList
            {
                Status = "Test2",
                Date = "1398/5/7"
            });

        }

        public ObservableCollection<CommentsList> Progresses { get; set; } = new ObservableCollection<CommentsList>();
        public long RequestCode { get; set; }

    }

    public class CommentsList
    {
        public string Status { get; set; }
        public string Date { get; set; }
    }
}
