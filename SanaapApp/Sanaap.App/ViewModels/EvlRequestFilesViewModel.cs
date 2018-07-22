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

        public Xamarin.Forms.ImageSource ImageSource { get; set; }

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

            TakePhoto = new BitDelegateCommand<FileListViewItem>(async (fileListViewItem) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به دوربین وجود ندارد", "باشه");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Small,
                    DefaultCamera = CameraDevice.Rear,
                    AllowCropping = true,
                });

                //await pageDialogService.DisplayAlertAsync("", file.Path, "باشه");

                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                if (file == null)
                {
                    return;
                }
                else
                {
                    fileListViewItem.imageSource = ImageSource;
                    //fileListViewItem.ImageStream = Helpers.Helpers.ConvertMediaFile64(file);
                }

                //await CrossMedia.Current.Initialize();

                //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                //{
                //    await pageDialogService.DisplayAlertAsync("", "دسترسی به دوربین وجود ندارد", "باشه");
                //    return;
                //}

                //MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                //{
                //    SaveToAlbum = true,
                //    PhotoSize = PhotoSize.Small,
                //    DefaultCamera = CameraDevice.Rear,
                //    AllowCropping = true,
                //    CompressionQuality = 0
                //});

                //ImageSource = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();
                //    return stream;
                //});

                //if (file == null)
                //{
                //    return;
                //}
                //else
                //{
                //    fileListViewItem.ImageSource = ImageSource;
                //    //fileListViewItem.FileType = Helpers.IAppUtilities.ConvertMediaFileToBase64(file);
                //}

                //fileListViewItem.ImageStream = file.GetStream();
                //ImageSource imageSource = ImageSource.FromStream(() => fileListViewItem.ImageStream);

                //evlRequestDto.fileListViewItems.Add(fileListViewItem);
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

                newImage.stream = file.GetStream();
                ImageSource imageSource = ImageSource.FromStream(() => newImage.stream);

                evlRequestDto.fileListViewItems.Add(newImage);
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

                fileListViewItems = new ObservableCollection<FileListViewItem>(fileTypes.Select(f => new FileListViewItem { evlRequestFileTypeDto = f }));

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
