using Bit.ViewModel;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestWaitViewModel : BitViewModelBase
    {
        public EvaluationRequestWaitViewModel()
        {
            string[] a = typeof(EvaluationRequestDescriptionViewModel).Assembly.GetManifestResourceNames();
        }
    }
}
