using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Enums;
using System;
using System.Net.Http;
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

            GotoMainInsurance = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(MainInsuranceView));
            });



            GotoEvlRequestMapBadane = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(EvaluationRequestDetailView), new NavigationParameters
                {
                    { "InsuranceType", InsuranceType.Badane }
                });
            });

            GotoDetail = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(EvaluationRequestDetailView), new NavigationParameters
                {
                    { "InsuranceType", InsuranceType.Sales}
                });
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

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            try
            {
                loginCancellationToken?.Cancel();
                loginCancellationToken = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: loginCancellationToken.Cancel))
                {
                    await syncInitialData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task syncInitialData()
        {
            await _initialDataService.GetCars();

            await _initialDataService.GetColors();

            await _initialDataService.GetCurrentUserInfo();

            await _initialDataService.GetInsurers();
        }
    }
}
