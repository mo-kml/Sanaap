using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
using Sanaap.Dto;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestExpertRankViewModel : BitViewModelBase
    {
        public EvlRequestItemSource Request { get; set; }

        public EvlRequestExpertDto Expert { get; set; }

        public BitDelegateCommand SubmitRank { get; set; }

        public EvaluationRequestExpertRankViewModel(IEvlRequestService requestService)
        {
            SubmitRank = new BitDelegateCommand(async () =>
              {
                  if (Request.RankValue != 0 || !string.IsNullOrEmpty(Request.RankDescription))
                  {
                      await requestService.UpdateRank(Request);
                  }

                  await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMenuView)}");
              });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));
            Expert = parameters.GetValue<EvlRequestExpertDto>(nameof(Expert));
        }
    }
}
