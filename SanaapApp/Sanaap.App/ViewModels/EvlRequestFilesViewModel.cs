using Acr.UserDialogs;
using Bit.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Simple.OData.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestFilesViewModel : BitViewModelBase
    {
        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

        private EvlRequestDto evlRequestDto;
        private readonly IODataClient _oDataClinet;
        private readonly INavigationService _navigationService;
        private readonly HttpClient _httpClient;
        private readonly IUserDialogs _userDialogs;

        public EvlRequestFilesViewModel(IMedia media, IODataClient oDataClient, INavigationService navigationService
            , HttpClient httpClient, IUserDialogs userDialogs, IPageDialogService pageDialogService)
        {
            _oDataClinet = oDataClient;
            _navigationService = navigationService;
            _httpClient = httpClient;
            _userDialogs = userDialogs;

            TakePhoto = new BitDelegateCommand<FileListViewItem>(async (newImage) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به دوربین وجود ندارد", "باشه");
                    return;
                }

                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Small,
                    DefaultCamera = CameraDevice.Rear,
                    AllowCropping = true,
                    CompressionQuality = 0
                });

                newImage.ImageStream = file.GetStream();
                ImageSource imageSource = ImageSource.FromStream(() => newImage.ImageStream);

                evlRequestDto.Images.Add(newImage);
            });

            PickFromGallery = new BitDelegateCommand<FileListViewItem>(async (newImage) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به تصاویر وجود ندارد", "باشه");
                    return;
                }

                MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    RotateImage = true,
                    CompressionQuality = 0
                });

                newImage.ImageStream = file.GetStream();
                ImageSource imageSource = ImageSource.FromStream(() => newImage.ImageStream);

                evlRequestDto.Images.Add(newImage);
            });

            Submit = new BitDelegateCommand(async () =>
            {
                if (await pageDialogService.DisplayAlertAsync("مطمئن هستید؟", "درخواست ارسال شود؟", "بله", "خیر"))
                {
                    using (userDialogs.Loading(ConstantStrings.SendingRequestAndPictures))
                    {
                        await navigationService.NavigateAsync("EvlRequestWait", new NavigationParameters
                        {
                            { "EvlRequestDto", evlRequestDto }
                        });
                    }
                }
            });
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto"); // Get Parameter

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                await CrossMedia.Current.Initialize();

                await base.OnNavigatedToAsync(parameters);
            }
        }

        public override Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            parameters.Add("EvlRequestDto", evlRequestDto);
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
