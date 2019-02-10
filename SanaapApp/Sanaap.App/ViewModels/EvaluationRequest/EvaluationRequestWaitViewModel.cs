using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestWaitViewModel : BitViewModelBase
    {
        private IEvlRequestService _evlRequestService;
        public EvaluationRequestWaitViewModel(IEvlRequestService evlRequestService)
        {
            _evlRequestService = evlRequestService;
        }

        public EvlRequestItemSource Request { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

            EvlRequestExpertDto expertDto = await _evlRequestService.FindEvlRequestExpert(Request.Id);
        }
    }
}
