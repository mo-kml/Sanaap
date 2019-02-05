using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class MainMenuViewModel : BitViewModelBase
    {
        public bool IsSynced { get; set; }
        private readonly IUserDialogs _userDialogs;
        private readonly HttpClient _httpClient;
        private readonly IInitialDataService _initialDataService;
        public MainMenuViewModel(
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs, HttpClient httpClient, IInitialDataService initialDataService)
        {
            _userDialogs = userDialogs;
            _httpClient = httpClient;
            _initialDataService = initialDataService;

            GoToEvalutionRequestMenu = new BitDelegateCommand(async () =>
              {
                  await NavigationService.NavigateAsync(nameof(EvaluationRequestMenuView));
              });

        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                loginCancellationToken?.Cancel();
                loginCancellationToken = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: loginCancellationToken.Cancel))
                {
                    if (IsSynced == false)
                    {
                        await syncInitialData();

                        IsSynced = true;
                    }
                }
            }
        }

        public CancellationTokenSource loginCancellationToken { get; set; }

        public BitDelegateCommand GoToEvalutionRequestMenu { get; set; }

        public async Task syncInitialData()
        {
            await _initialDataService.GetCars();

            await _initialDataService.GetColors();

            await _initialDataService.GetCurrentUserInfo();

            await _initialDataService.GetInsurers();

            await _initialDataService.GetAlphabets();
        }
    }
}
