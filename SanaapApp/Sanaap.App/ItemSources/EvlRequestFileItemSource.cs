using PropertyChanged;
using Sanaap.App.Dto;
using Xamarin.Forms;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class EvlRequestFileItemSource : EvlRequestFileDto
    {
        public ImageSource Image { get; set; }

        public string TypeName { get; set; }

        public bool HasImage { get; set; }

        public bool IsRequired { get; set; }
    }
}
