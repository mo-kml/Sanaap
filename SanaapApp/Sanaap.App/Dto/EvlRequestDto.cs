using FFImageLoading.Work;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sanaap.App.Dto
{
    public class FileListViewItem : BindableBase
    {
        public EvlRequestFileTypeDto FileType { get; set; } = new EvlRequestFileTypeDto();

        public ImageSource ItemSource { get; set; }

        public Stream ImageStream { get; set; }

        public bool IsVisible { get; set; }
    }

    public partial class EvlRequestDto
    {
        [NotMapped]
        public List<FileListViewItem> fileListViewItems { get; set; } = new List<FileListViewItem>();
    }
}
