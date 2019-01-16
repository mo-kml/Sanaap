using Bit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sanaap.App.ViewModels.News
{
    public class NewsListViewModel : BitViewModelBase
    {
        public NewsListViewModel()
        {

        }

        public ObservableCollection<News> NewsSource { get; set; } = new ObservableCollection<News>();

    }

    //Must Be Added To Dto
    public class News
    {
        public string Image { get; set; }
        public string Header { get; set; }
        public string MainText { get; set; }
        public bool IsLiked { get; set; }
        public string CreatedDate { get; set; }
        public int LikeCount { get; set; }
    }

}
