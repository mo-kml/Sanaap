using Acr.UserDialogs;
using Bit.ViewModel;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvaluationRequestFilesViewModel : BitViewModelBase
    {
        public BitDelegateCommand<EvlRequestFile> TakePhoto { get; set; }

        public BitDelegateCommand<EvlRequestFile> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public List<EvlRequestFile> Files { get; set; }

        public EvlRequestDto Request { get; set; }

        private readonly IODataClient _oDataClient;
        private readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly HttpClient _httpClient;

        public EvaluationRequestFilesViewModel(IMedia media,
            IODataClient oDataClient,
            INavigationService navigationService,
            IUserDialogs userDialogs,
            IPageDialogService pageDialogService,
            HttpClient httpClient)
        {
            _oDataClient = oDataClient;
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _httpClient = httpClient;

            TakePhoto = new BitDelegateCommand<EvlRequestFile>(async (file) =>
            {
                if (!CrossMedia.Current.IsTakePhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به دوربین وجود ندارد", "باشه");
                    return;
                }

                MediaFile newFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true,
                    DefaultCamera = CameraDevice.Rear,
                    AllowCropping = true,
                    RotateImage = true,
                    CompressionQuality = 25
                });

                if (newFile == null)
                {
                    return;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream stream = newFile.GetStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        file.Data = memoryStream.ToArray();
                    }
                }

            });

            PickFromGallery = new BitDelegateCommand<EvlRequestFile>(async (file) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به تصاویر وجود ندارد", "باشه");
                    return;
                }

                MediaFile newFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    RotateImage = true,
                    CompressionQuality = 25
                });

                if (newFile == null)
                {
                    return;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream stream = newFile.GetStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        file.Data = memoryStream.ToArray();
                    }
                }

            });

            Submit = new BitDelegateCommand(async () =>
            {
                if (await pageDialogService.DisplayAlertAsync("مطمئن هستید؟", "درخواست ارسال شود؟", "بله", "خیر"))
                {
                    using (userDialogs.Loading(ConstantStrings.SendingRequestAndPictures))
                    {
                        MultipartFormDataContent submitEvlRequestContents = new MultipartFormDataContent
                        {
                            new StringContent(JsonConvert.SerializeObject(Request), Encoding.UTF8, "application/json")
                        };

                        foreach (EvlRequestFile file in Files.Where(f => f.Data != null))
                        {
                            submitEvlRequestContents.Add(new ByteArrayContent(file.Data), file.FileType.Id.ToString(), file.FileType.Id.ToString());
                        }

                        HttpRequestMessage submitEvlRequest = new HttpRequestMessage(HttpMethod.Post, "api/evl-requests/submit-evl-request")
                        {
                            Content = submitEvlRequestContents
                        };

                        HttpResponseMessage submitEvlRequestExpertResponse = await _httpClient.SendAsync(submitEvlRequest);

                        submitEvlRequestExpertResponse.EnsureSuccessStatusCode();

                        Request = JsonConvert.DeserializeObject<EvlRequestDto>(await submitEvlRequestExpertResponse.Content.ReadAsStringAsync());

                        await navigationService.NavigateAsync("EvlRequestWait", new NavigationParameters
                        {
                            { "EvlRequestDto", Request }
                        });
                    }
                }
            });
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                Request = parameters.GetValue<EvlRequestDto>(nameof(EvlRequestDto));

                parameters.TryGetValue(nameof(Position), out Position position);

                Request.Latitude = position.Latitude;
                Request.Longitude = position.Longitude;

                EvlRequestFileTypeDto[] fileTypes = (await _oDataClient.For<EvlRequestFileTypeDto>("EvlRequestFileTypes").FindEntriesAsync()).ToArray();

                Files = new List<EvlRequestFile>(fileTypes.Select(fileType => new EvlRequestFile { FileType = fileType }));

                await base.OnNavigatedToAsync(parameters);
            }
        }

        public override Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            parameters.Add(nameof(EvlRequestDto), Request);
            parameters.Add(nameof(Position), new Position(Request.Latitude, Request.Longitude));
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
