using Prism.Navigation;
using Xamarin.Forms;

namespace Sanaap.App.Views
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
