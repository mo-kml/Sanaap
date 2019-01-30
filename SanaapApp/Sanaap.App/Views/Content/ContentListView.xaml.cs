using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.Content
{
    
    public partial class ContentListView : ContentPage
    {
        public ContentListView()
        {
            InitializeComponent();
        }

        private void OpenFilter(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
    }
}
