using Bit.ViewModel;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestExpertViewModel : BitViewModelBase
    {
        public EvaluationRequestExpertViewModel()
        {
            Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg");
        }

        public ImageSource Image { get; set; }
    }
}
