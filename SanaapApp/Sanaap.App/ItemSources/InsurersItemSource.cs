using PropertyChanged;
using Sanaap.Dto;
using Xamarin.Forms;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class InsurersItemSource : InsurerDto
    {
        public ImageSource Image { get; set; }

        public bool IsSelected { get; set; }
    }
}
