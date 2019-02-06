using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.Dto;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Comment
{
    public class CommentAnswerPopupViewModel : BitViewModelBase
    {
        public CommentAnswerPopupViewModel()
        {
            ClosePopup = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync();
              });
        }
        public override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Comment = parameters.GetValue<CommentDto>(nameof(Comment));

            return base.OnNavigatedToAsync(parameters);
        }

        public CommentDto Comment { get; set; }

        public BitDelegateCommand ClosePopup { get; set; }

    }
}
