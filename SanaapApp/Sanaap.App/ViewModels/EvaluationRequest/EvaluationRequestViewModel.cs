using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Views.EvaluationRequest;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestViewModel : BitViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public EvaluationRequestViewModel(IEventAggregator eventAggregator)
        {
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
                await NavigationService.NavigateAsync(nameof(EvaluationRequestDetailView), parameters);
            }
        }
    }
}
