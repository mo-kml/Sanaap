using Acr.UserDialogs;
using Bit.ViewModel;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Simple.OData.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestFilesViewModel : BitViewModelBase
    {
        public BitDelegateCommand<EvlRequestFile> TakePhoto { get; set; }

        public BitDelegateCommand<EvlRequestFile> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public List<EvlRequestFile> Files { get; set; }

        private EvlRequestDto _evlRequest;
        private readonly IODataClient _oDataClient;
        private readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly HttpClient _httpClient;

        public EvlRequestFilesViewModel(IMedia media,
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

                using (MediaFile newFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    RotateImage = true,
                    CompressionQuality = 0
                }))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (Stream stream = newFile.GetStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            file.Data = memoryStream.ToArray();
                        }
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

                using (MediaFile newFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    RotateImage = true,
                    CompressionQuality = 0
                }))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (Stream stream = newFile.GetStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            file.Data = memoryStream.ToArray();
                        }
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
                            new StringContent(JsonConvert.SerializeObject(_evlRequest), Encoding.UTF8, "application/json")
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

                        _evlRequest = JsonConvert.DeserializeObject<EvlRequestDto>(await submitEvlRequestExpertResponse.Content.ReadAsStringAsync());

                        await navigationService.NavigateAsync("EvlRequestWait", new NavigationParameters
                        {
                            { "EvlRequestDto", _evlRequest }
                        });
                    }
                }
            });
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                _evlRequest = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

                EvlRequestFileTypeDto[] fileTypes = (await _oDataClient.For<EvlRequestFileTypeDto>("EvlRequestFileTypes").FindEntriesAsync()).ToArray();

                Files = new List<EvlRequestFile>(fileTypes.Select(fileType => new EvlRequestFile { FileType = fileType }));

                await base.OnNavigatedToAsync(parameters);
            }
        }

        public override Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            parameters.Add("EvlRequestDto", _evlRequest);
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
