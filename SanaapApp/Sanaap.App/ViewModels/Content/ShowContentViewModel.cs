using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Content
{
    public class ShowContentViewModel : BitViewModelBase
    {
        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        public ShowContentViewModel(IODataClient oDataClient, IUserDialogs userDialogs)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Guid contentId;

            parameters.TryGetValue("ContentId", out contentId);

            using (_userDialogs.Loading())
            {
                await loadContent(contentId);
            }
        }

        public async Task loadContent(Guid contentId)
        {
            Content = await _oDataClient.For<ContentDto>("Contents")
                    .Key(contentId)
                    .FindEntryAsync();
        }
        public ContentDto Content { get; set; }


    }
}
