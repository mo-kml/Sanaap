using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Events;
using Prism.Navigation;
using Sanaap.App.Events;
using Sanaap.App.Views.EvaluationRequest;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestViewModel : BitViewModelBase
    {
        private readonly IPopupNavigationService _popupNavigationService;

        public EvaluationRequestViewModel(IPopupNavigationService popupNavigationService, IEventAggregator eventAggregator)
        {
            _popupNavigationService = popupNavigationService;

            eventAggregator.GetEvent<InsuranceEvent>().SubscribeAsync(async (insurance) =>
            {
                await _popupNavigationService.PushAsync(nameof(EvaluationRequestDetailView));
            });
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            await _popupNavigationService.PushAsync(nameof(EvaluationRequestDetailView));
        }


    }
}
