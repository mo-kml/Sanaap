using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestWaitViewModel : BitViewModelBase
    {
        private readonly IEvlRequestService _evlRequestService;
        private readonly IPageDialogService _dialogService;
        public EvaluationRequestWaitViewModel(IEvlRequestService evlRequestService, IPageDialogService dialogService)
        {
            _evlRequestService = evlRequestService;
            _dialogService = dialogService;

            Cancel = new BitDelegateCommand(async () =>
              {
                  if (await dialogService.DisplayAlertAsync(string.Empty, ConstantStrings.AreYouSureToCancel, ConstantStrings.Yes, ConstantStrings.No))
                  {
                      await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMenuView)}");
                  }
              });
        }

        public EvlRequestItemSource Request { get; set; }

        public BitDelegateCommand Cancel { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

            EvlRequestExpertDto expertDto = null;

            try
            {
                expertDto = await _evlRequestService.FindEvlRequestExpert(Request.Id);

                Request.Code = expertDto.FileID;
            }
            catch (System.Exception)
            {
            }

            if (expertDto?.Expert == null)
            {
                await _dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.FindNearExpertError + System.Environment.NewLine + Request.Code, ConstantStrings.Ok);

                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMenuView)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(EvaluationRequestExpertView)}", new NavigationParameters
                {
                    {"Expert",expertDto }
                });
            }
        }
    }
}
