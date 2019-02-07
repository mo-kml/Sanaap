using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Services.Implementations;
using Sanaap.Constants;
using Simple.OData.Client;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Sanaap.App.ViewModels.Content
{
    public class ShowContentViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        private readonly INewsService _newsService;
        public ShowContentViewModel(IODataClient oDataClient, IUserDialogs userDialogs, INewsService newsService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _newsService = newsService;


            Like = new BitDelegateCommand(async () =>
              {
                  using (_userDialogs.Loading(ConstantStrings.Loading))
                  {
                      bool likeStatus = Content.YourLike;

                      Content.YourLike = await newsService.LikeNews(Content.NewsID);

                      if (Content.YourLike == false && likeStatus == true)
                      {
                          Content.Likes--;
                      }
                      else if (Content.YourLike == true && likeStatus == false)
                      {
                          Content.Likes++;
                      }
                  }
              });

            ShareCommand = new BitDelegateCommand(async () =>
             {
                 await Share.RequestAsync(new ShareTextRequest
                 {
                     Text = NewsService.StripHTML(Content.Text),
                     Title = Content.Title
                 });
             });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            int newsId;

            parameters.TryGetValue("NewsId", out newsId);

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                await loadContent(newsId);
            }
        }

        public async Task loadContent(int newsId)
        {
            Content = await _newsService.GetNewsById(newsId);
        }

        public BitDelegateCommand Like { get; set; }

        public BitDelegateCommand ShareCommand { get; set; }

        public NewsItemSource Content { get; set; }


    }
}
