using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDMaster : ContentPage
    {
        public ListView ListView;

        public MDMaster()
        {
            InitializeComponent();

            BindingContext = new MDMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MDMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MDMenuItem> MenuItems { get; set; }
            
            public MDMasterViewModel()
            {
                MenuItems = new ObservableCollection<MDMenuItem>(new[]
                {
                    new MDMenuItem { Id = 0, Title = "Page 1" },
                    new MDMenuItem { Id = 1, Title = "Page 2" },
                    new MDMenuItem { Id = 2, Title = "Page 3" },
                    new MDMenuItem { Id = 3, Title = "Page 4" },
                    new MDMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}