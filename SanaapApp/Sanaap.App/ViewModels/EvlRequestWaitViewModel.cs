using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestWaitViewModel : BitViewModelBase, IDestructible
    {
        private readonly HttpClient _httpClient;
        private readonly IClientAppProfile _clientAppProfile;

        private readonly IODataClient _odataClient;
        private EvlRequestDto _evlRequest;

        public string ExpertFullName { get; set; }
        public string ExpertMobileNo { get; set; }
        public ImageSource ExpertImage { get; set; }

        public BitDelegateCommand GoToMain { get; set; }
        public BitDelegateCommand Call { get; set; }
        public virtual BitDelegateCommand<int> RatingValueChanged { get; set; }

        public bool IsVisibleBefore { get; set; } = true;
        public bool IsVisibleAfter { get; set; } = false;

        public string Message { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        public EvlRequestWaitViewModel(INavigationService navigationService,
            IODataClient odataClient,
            IDeviceService deviceService,
            IODataClient oDataClient,
            IPageDialogService pageDialogService,
            HttpClient httpClient,
            IClientAppProfile clientAppProfile)
        {
            _odataClient = odataClient;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _httpClient = httpClient;
            _clientAppProfile = clientAppProfile;

            GoToMain = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/Menu/Nav/Main");
            });

            Call = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://" + ExpertMobileNo + ""));
            });

            //RatingValueChanged = new BitDelegateCommand<int>(async (value) =>
            //{
            //    await _pageDialogService.DisplayAlertAsync("", "RatingValueChanged : " + value.ToString(), ErrorMessages.Ok);
            //});
        }

        public async override Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            _evlRequest = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

            Message = ConstantStrings.ExpertFinding;
            IsVisibleBefore = true;
            IsVisibleAfter = false;

            EvlRequestExpertDto evlRequestExpert = null;
            try
            {
                _httpClient.BaseAddress = new Uri($"{_clientAppProfile.HostUri}");

                string evlRequestIdJson = JsonConvert.SerializeObject(_evlRequest.Id);
                var stringContent = new StringContent(evlRequestIdJson, UnicodeEncoding.UTF8, "application/json");

                evlRequestExpert = JsonConvert.DeserializeObject<EvlRequestExpertDto>(await (await _httpClient.PostAsync(
                    _httpClient.BaseAddress + "api/EvlRequestExperts/FindNearExpert", stringContent)).Content.ReadAsStringAsync());


                //evlRequestExpert = await _odataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
                //    .Function("FindEvlRequestExpert")
                //    .FindEntryAsync();
            }
            catch (Exception ex)
            {
                await _navigationService.NavigateAsync("/Menu/Nav/Main");
                await _pageDialogService.DisplayAlertAsync("", ConstantStrings.FindNearExpertError, ErrorMessages.Ok);
            }

            IsVisibleBefore = false;
            IsVisibleAfter = true;

            Message = ConstantStrings.ExpertFind;
            if (evlRequestExpert != null)
            {
                ExpertFullName = evlRequestExpert.Expert.Name.Trim();
                ExpertMobileNo = evlRequestExpert.Expert.Mobile.Trim();
                byte[] imageAsBytes = Convert.FromBase64String(evlRequestExpert.Expert.Photo.Split(',')[1]);
                ExpertImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            await base.OnNavigatingToAsync(parameters);
        }
    }
}
