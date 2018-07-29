using Prism.Mvvm;

namespace Sanaap.App.Dto
{
    public class EvlRequestFile : BindableBase
    {
        public byte[] Data { get; set; }

        public EvlRequestFileTypeDto FileType { get; set; }
    }
}
