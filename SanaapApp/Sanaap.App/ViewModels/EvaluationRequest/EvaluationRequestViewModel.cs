using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Sanaap.App.Views.EvaluationRequest;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestViewModel : BitViewModelBase
    {
        private readonly IPopupNavigationService _popupNavigationService;
        public EvaluationRequestViewModel(IPopupNavigationService popupNavigationService)
        {
            _popupNavigationService = popupNavigationService;
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            await _popupNavigationService.PushAsync(nameof(EvaluationRequestLostDetailView));
        }
    }
}
