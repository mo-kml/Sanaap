using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.Views.Content;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
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

            ShowContent = new BitDelegateCommand<ContentListDto>(async (content) =>
              {
                  INavigationParameters parameters = new NavigationParameters();
                  parameters.Add("ContentId", Guid.Parse("890f984f-f5aa-4cd3-870a-02f9e15e1037"));

                  await NavigationService.NavigateAsync(nameof(ShowContentView), parameters);
              });
        }
        public ObservableCollection<ContentListDto> Contents { get; set; }

        public BitDelegateCommand<ContentListDto> ShowContent { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
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
