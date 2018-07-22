using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sanaap.App.Dto
{
    [AddINotifyPropertyChangedInterface]
    public class FileListViewItem
    {
        public EvlRequestFileTypeDto evlRequestFileTypeDto { get; set; } = new EvlRequestFileTypeDto();

        public Xamarin.Forms.ImageSource imageSource { get; set; }

        public Stream stream { get; set; }

        public bool isVisible { get; set; }
    }

    public partial class EvlRequestDto
    {
        [NotMapped]
        public List<FileListViewItem> fileListViewItems { get; set; } = new List<FileListViewItem>();
    }
}
