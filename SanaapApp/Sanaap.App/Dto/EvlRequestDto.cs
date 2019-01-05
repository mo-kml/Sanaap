using Prism.Mvvm;

namespace Sanaap.App.Dto
{
    public class EvlRequestFileDto : BindableBase
    {
        public byte[] Data { get; set; }

        public int TypeId { get; set; }

    }
}
