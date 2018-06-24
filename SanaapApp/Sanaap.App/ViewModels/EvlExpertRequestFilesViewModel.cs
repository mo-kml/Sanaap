using Bit.ViewModel;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Sanaap.App.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestFilesViewModel : BitViewModelBase
    {
        public EvlExpertRequestFilesViewModel(IMedia media)
        {
            TakePhoto = new BitDelegateCommand<FileListViewItem>(async (parameter) =>
              {
                  var image = await media.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true, DefaultCamera = CameraDevice.Rear, RotateImage = true });

                  var items = FileTypes;

                  var item = items.FirstOrDefault(c => c.Id == parameter.Id);

                  item.ItemSource = ImageSource.FromStream(() => image.GetStream());

                  FileTypes = items;

              });

            PickFromGallery = new BitDelegateCommand<FileListViewItem>(async (paramter) =>
            {
                var a = await media.PickPhotoAsync(new PickMediaOptions { RotateImage = true });

                paramter.ItemSource = ImageSource.FromStream(() => a.GetStream());
            });
        }
        public ObservableCollection<FileListViewItem> FileTypes { get; set; }

        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            List<FileListViewItem> files = new List<FileListViewItem>
            {
                new FileListViewItem{Id=Guid.NewGuid(), Name="کد ملی"},
                new FileListViewItem{Id=Guid.NewGuid(), Name="شناسنامه"},
                new FileListViewItem{Id=Guid.NewGuid(), Name="کد ملی"},
                new FileListViewItem{Id=Guid.NewGuid(), Name="کد ملی"},
            };

            FileTypes = new ObservableCollection<FileListViewItem>(files);

            base.OnNavigatedTo(parameters);
        }
    }

    public class FileListViewItem : FileTypeDto
    {
        public ImageSource ItemSource { get; set; }
    }
}
