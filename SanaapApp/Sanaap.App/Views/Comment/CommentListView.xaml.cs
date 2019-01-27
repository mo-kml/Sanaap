using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.Comment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentListView : ContentPage
    {
        public CommentListView()
        {
            InitializeComponent();
        }

        private void OpenDrawer(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
    }
}
