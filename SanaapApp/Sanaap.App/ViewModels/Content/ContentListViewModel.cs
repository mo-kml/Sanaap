using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.Content;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Content
{
    public class ContentListViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        private readonly INewsService _newsService;
        public ContentListViewModel(IODataClient oDataClient, IUserDialogs userDialogs, INewsService newsService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _newsService = newsService;

            ShowContent = new BitDelegateCommand<NewsItemSource>(async (content) =>
              {
                  INavigationParameters parameters = new NavigationParameters();
                  parameters.Add("NewsId", content.NewsID);

                  await NavigationService.NavigateAsync(nameof(ShowContentView), parameters);
              });
        }
        public ObservableCollection<NewsItemSource> Contents { get; set; }

        public BitDelegateCommand<NewsItemSource> ShowContent { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            using (_userDialogs.Loading())
            {
                await loadContents();
            }
        }

        public async Task loadContents()
        {
            List<NewsItemSource> contents = await _newsService.GetNews();

            if (contents != null)
            {
                Contents = new ObservableCollection<NewsItemSource>(contents);
            }
        }
    }
}
