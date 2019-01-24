using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Simple.OData.Client;
using System.Threading.Tasks;

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
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            int newsId;

            parameters.TryGetValue("NewsId", out newsId);

            using (_userDialogs.Loading())
            {
                await loadContent(newsId);
            }
        }

        public async Task loadContent(int newsId)
        {
            Content = await _newsService.GetNewsById(newsId);
        }

        public NewsItemSource Content { get; set; }


    }
}
