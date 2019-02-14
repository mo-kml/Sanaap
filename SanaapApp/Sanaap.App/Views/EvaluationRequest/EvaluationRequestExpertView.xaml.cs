using Bit.ViewModel.Contracts;
using Prism.Services;
using Sanaap.Constants;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.Views.EvaluationRequest
{

    public partial class EvaluationRequestExpertView : ContentPage
    {
        private IPageDialogService _dialogService;
        private INavService _navService;
        public EvaluationRequestExpertView(IPageDialogService dialogService, INavService navService)
        {
            _dialogService = dialogService;
            _navService = navService;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Task.Run(async () =>
            {
                if (await _dialogService.DisplayAlertAsync(string.Empty, ConstantStrings.AreYouSureToCancel, ConstantStrings.Yes, ConstantStrings.No))
                {
                    await _navService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMenuView)}");
                }
            });
            return true;
        }
    }
}
