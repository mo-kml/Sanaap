using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.Dto;
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
        public ContentListViewModel(IODataClient oDataClient, IUserDialogs userDialogs)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
        }
        public ObservableCollection<ContentListDto> Contents { get; set; }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (_userDialogs.Loading())
            {
                await loadContents();
            }
        }

        public async Task loadContents()
        {
            IEnumerable<ContentListDto> contents = await _oDataClient.For<ContentListDto>("ContentLists")
                    .Function("GetAllContents")
                    .FindEntriesAsync();

            if (contents != null)
            {
                Contents = new ObservableCollection<ContentListDto>(contents);
            }
        }
    }
}
