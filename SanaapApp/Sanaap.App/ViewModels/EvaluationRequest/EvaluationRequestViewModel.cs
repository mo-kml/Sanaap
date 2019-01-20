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
        private readonly INavService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        public EvaluationRequestViewModel(INavService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            //if (parameters.TryGetValue("IsOpenInsurance", out bool isOpen))
            //{
            //    await NavigationService.GoBackAsync(new NavigationParameters
            //      {
            //          {"Request",parameters.GetValue<EvlRequestItemSource>("Request") },
            //          { "OpenInsurancePopup",true }
            //      });
            //}
            //else
            //{
            //    await _navigationService.NavigateAsync(nameof(EvaluationRequestDetailView), parameters);
            //}


            await _navigationService.NavigateAsync(nameof(EvaluationRequestDetailView), parameters);

        }
    }
}
