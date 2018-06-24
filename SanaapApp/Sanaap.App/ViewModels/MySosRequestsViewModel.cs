using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Linq;

namespace Sanaap.App.ViewModels
{
    public class MySosRequestsViewModel : BitViewModelBase
    {
        private readonly IODataClient _odataClient;
        private IConnectivity _connectivity;
        private readonly IPageDialogService _pageDialogService;

        public bool IsBusy { get; set; }

        public SosRequestDto[] MySosRequests { get; set; }

        public MySosRequestsViewModel(
            INavigationService navigationService, IODataClient odataClient,
            IConnectivity connectivity, IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
            _connectivity = connectivity;
            _pageDialogService = pageDialogService;

            GoBack = new BitDelegateCommand(() =>
            {
                navigationService.GoBackAsync();
            });
        }

        public BitDelegateCommand GoBack { get; set; }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            IsBusy = true;

            try
            {
                if (_connectivity.IsConnected == false)
                {
                    await _pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                    return;
                }

                MySosRequests = (await _odataClient.For<SosRequestDto>("SosRequests")
                    .Function("GetMySosRequests")
                    .OrderBy(it => it.ModifiedOn)
                    .FindEntriesAsync())
                    .ToArray();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await _pageDialogService.DisplayAlertAsync("", "خطای نامشخص", "باشه");
            }
            finally
            {
                base.OnNavigatedTo(parameters);

                IsBusy = false;
            }
        }
    }
}
