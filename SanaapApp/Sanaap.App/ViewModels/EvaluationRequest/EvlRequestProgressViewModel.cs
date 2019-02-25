using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvlRequestProgressViewModel : BitViewModelBase
    {
        private readonly IEvlRequestService _evlRequestService;
        private readonly IUserDialogs _userDialogs;
        private readonly IDateHelper _dateHelper;
        public EvlRequestProgressViewModel(IEvlRequestService evlRequestService, IUserDialogs userDialogs, IDateHelper dateHelper)
        {
            _evlRequestService = evlRequestService;
            _userDialogs = userDialogs;
            _dateHelper = dateHelper;

            ClosePopup = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync();
              });
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            parameters.TryGetValue(nameof(EvlRequestListItemSource), out EvlRequestListItemSource request);

            RequestCode = request.Code;

            progressCancellationTokenSource?.Cancel();
            progressCancellationTokenSource = new CancellationTokenSource();
            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: progressCancellationTokenSource.Cancel))
            {
                await loadProgresses(request.Code);
            }
        }

        public ObservableCollection<ProgressItemSource> Progresses { get; set; }

        public long RequestCode { get; set; }

        private CancellationTokenSource progressCancellationTokenSource;

        public BitDelegateCommand ClosePopup { get; set; }

        public async Task loadProgresses(int fileId)
        {
            try
            {
                Progresses = new ObservableCollection<ProgressItemSource>(await _evlRequestService.GetAllProgressesByRequestId(fileId));
            }
            catch (System.Exception) { }
        }
    }
}
