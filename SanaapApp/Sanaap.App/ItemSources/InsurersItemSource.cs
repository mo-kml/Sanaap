using PropertyChanged;
using Xamarin.Forms;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class InsurersItemSource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ImageSource Image { get; set; }
    }
}
