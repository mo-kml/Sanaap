using Bit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sanaap.App.ViewModels.TheFiles
{
    public class TheFilesViewModel : BitViewModelBase
    {

        public TheFilesViewModel()
        {

        }

        public ObservableCollection<FilesItemSource> FilesList { get; set; } = new ObservableCollection<FilesItemSource>();

    }

    //Must Be Added In Dto

    public class FilesItemSource
    {
        public long Code { get; set; }
        public string Image { get; set; }
        public string CreatedDate { get; set; }
        public String FileType { get; set; }
    }

}
