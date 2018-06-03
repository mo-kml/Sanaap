using Bit.ViewModel.Implementations;

namespace SanaapOperatorApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new SanaapOperatorApp.App(new SanaapOperatorAppUWPInitializer()));
        }
    }

    public class SanaapOperatorAppUWPInitializer : BitPlatformInitializer
    {

    }
}
