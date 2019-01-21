using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Events;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Views;
using Sanaap.App.Views.EvaluationRequest;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestViewModel : BitViewModelBase
    {
        private readonly INavService _NavigationService;
        private readonly IEventAggregator _eventAggregator;
        public EvaluationRequestViewModel( IEventAggregator eventAggregator)
        {
            _NavigationService = NavigationService;
            _eventAggregator = eventAggregator;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("IsOpenInsurance", out bool isOpen))
            {
                await NavigationService.GoBackAsync(new NavigationParameters
                  {
                      {"Request",parameters.GetValue<EvlRequestItemSource>("Request") },
                      { "OpenInsurancePopup",true }
                  });
            }
            else
            {
                await _NavigationService.NavigateAsync(nameof(EvaluationRequestDetailView), parameters);
            }
        }
    }
}
