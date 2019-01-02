using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using Sanaap.Enums;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SosRequest { get; set; }

        public BitDelegateCommand GotoMainInsurance { get; set; }
        public BitDelegateCommand GotoEvlRequestMapBadane { get; set; }
        public BitDelegateCommand GotoDetail { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public CancellationTokenSource loginCancellationToken { get; set; }

        private readonly IUserDialogs _userDialogs;
        private readonly HttpClient _httpClient;
        private readonly IInitialDataService _initialDataService;
        public MainViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs, HttpClient httpClient, IInitialDataService initialDataService)
        {
            _userDialogs = userDialogs;
            _httpClient = httpClient;
            _initialDataService = initialDataService;

            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

            GotoMainInsurance = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MainInsurance");
            });

            GotoEvlRequestMapBadane = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequestDetail", new NavigationParameters
                {
                    { "InsuranceType", InsuranceType.Badane }
                });
            });

            GotoDetail = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequestDetail");
            });

            Logout = new BitDelegateCommand(async () =>
            {
                await securityService.Logout();
                await navigationService.NavigateAsync("/Login");
            });

            SosRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("SosRequest");
            });

            SubmitSosRequestByCall = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://0211401"));
            });
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            try
            {
                loginCancellationToken?.Cancel();
                loginCancellationToken = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: loginCancellationToken.Cancel))
                {
                    await loginToExternalProject();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task loginToExternalProject()
        {
            string json = JsonConvert.SerializeObject(new { username = "sanap", password = "10431044" });

            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage result = await _httpClient.PostAsync("Login", stringContent);

            if (result.IsSuccessStatusCode)
            {
                JObject credential = JObject.Parse(await result.Content.ReadAsStringAsync());

                _httpClient.DefaultRequestHeaders.Add("auth", credential["token"].ToString());

                await syncInitialData();
            }
        }

        public async Task syncInitialData()
        {
            await _initialDataService.GetCars();

            await _initialDataService.GetColors();
        }
    }
}
