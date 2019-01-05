using PropertyChanged;
using Sanaap.Dto;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class EvlRequestItemSource : EvlRequestDto
    {
        public bool IsSales { get; set; }
    }
}
