using Acr.UserDialogs;
using Bit.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Simple.OData.Client;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestFilesViewModel : BitViewModelBase
    {
        public ObservableCollection<FileListViewItem> fileListViewItems { get; set; }

        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

        private EvlRequestDto evlRequestDto;
        private readonly IODataClient oDataClient;
        private readonly INavigationService _navigationService;
        private readonly HttpClient _httpClient;
        private readonly IUserDialogs _userDialogs;

        public EvlRequestFilesViewModel(IMedia media, IODataClient oDataClient, INavigationService navigationService
            , HttpClient httpClient, IUserDialogs userDialogs, IPageDialogService pageDialogService)
        {
            this.oDataClient = oDataClient;
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
            await CrossMedia.Current.Initialize();

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto"); // Get Parameter

                string path = _navigationService.GetNavigationUriPath();

                //var a = await _oDataClinet.For<FileTypeDto>("FileTypes")
                //          .FindEntriesAsync();

                EvlRequestFileTypeDto[] fileTypes = (await oDataClient.For<EvlRequestFileTypeDto>("EvlRequestFileTypes").FindEntriesAsync()).ToArray();
                //JsonConvert.DeserializeObject<EvlRequestFileTypeDto[]>(await _httpClient.GetStringAsync(_httpClient.BaseAddress + "api/FileTypes/GetAll"));

                fileListViewItems = new ObservableCollection<FileListViewItem>(fileTypes.Select(f => new FileListViewItem { FileType = f }));

                base.OnNavigatedTo(parameters);
            }


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
