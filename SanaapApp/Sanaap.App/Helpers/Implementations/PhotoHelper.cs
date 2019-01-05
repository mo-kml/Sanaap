using Plugin.Media.Abstractions;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using System.Threading.Tasks;

namespace Sanaap.App.Helpers.Implementations
{
    public class PhotoHelper : IPhotoHelper
    {
        private readonly IMedia _media;
        private readonly IPageDialogService _dialogService;
        public PhotoHelper(IPageDialogService dialogService, IMedia media)
        {
            _media = media;
            _dialogService = dialogService;
        }

        public async Task<MediaFile> TakePhoto()
        {
            MediaFile photo;

            if (!_media.IsTakePhotoSupported)
            {
                await _dialogService.DisplayAlertAsync("", "دسترسی به دوربین وجود ندارد", "باشه");
                return null;
            }

            bool isCamera = await _dialogService.DisplayAlertAsync("", "لطفا روش آپلود عکس را انتخاب نمایید", "باز کردن دوربین", "انتخاب از گالری");

            if (isCamera)
            {
                photo = await _media.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 10,
                    CustomPhotoSize = 10,
                    PhotoSize = PhotoSize.Medium,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front
                });
            }
            else
            {

                photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 10,
                });
            }

            if (photo == null)
            {
                await _dialogService.DisplayAlertAsync("خطا", "تصویر انتخاب نشده است", "باشه");
            }
            else
            {
                return photo;
            }

            return null;
        }
    }
}
