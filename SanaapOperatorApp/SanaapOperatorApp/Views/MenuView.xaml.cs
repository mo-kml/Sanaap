using Prism.Navigation;
using Xamarin.Forms;

namespace SanaapOperatorApp.Views
{
    public partial class MenuView : MasterDetailPage, IMasterDetailPageOptions
    {
        public MenuView()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation => false;
    }
}
