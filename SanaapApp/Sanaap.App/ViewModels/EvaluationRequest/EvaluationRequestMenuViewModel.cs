using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Enums;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestMenuViewModel : BitViewModelBase
    {
        private IEventAggregator _eventAggregator;
        public EvaluationRequestMenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            EvlRequestBadane = new BitDelegateCommand(async () =>
              {
                  await NavigationService.NavigateAsync(nameof(EvaluationRequestView), new NavigationParameters
                  {
                    { nameof(InsuranceType), InsuranceType.Badane }
                  });
              });

            EvlRequestSales = new BitDelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync(nameof(EvaluationRequestView), new NavigationParameters
                {
                    { nameof(InsuranceType), InsuranceType.Sales }
                });
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {

            if (parameters.TryGetValue("OpenInsurancePopup", out bool isOpen))
            {
                _eventAggregator.GetEvent<OpenInsurancePopupEvent>().Publish(new OpenInsurancePopupEvent());

                _eventAggregator.GetEvent<InsuranceEvent>().Publish(parameters.GetValue<EvlRequestItemSource>("Request"));
            }
        }
        public BitDelegateCommand EvlRequestSales { get; set; }

        public BitDelegateCommand EvlRequestBadane { get; set; }
    }
}
