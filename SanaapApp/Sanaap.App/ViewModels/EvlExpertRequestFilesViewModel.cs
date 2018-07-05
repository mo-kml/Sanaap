using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using PropertyChanged;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestFilesViewModel : BitViewModelBase
    {
        public ObservableCollection<FileListViewItem> FileTypes { get; set; }

        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public ImageSource ImageSource { get; set; }

        private EvlExpertRequestDto evlExpertRequestDto;
        private readonly IODataClient _oDataClinet;
        private readonly INavigationService _navigationService;
        private readonly HttpClient _httpClient;
        private readonly IClientAppProfile _clientAppProfile;
        private readonly IUserDialogs _userDialogs;

        public EvlExpertRequestFilesViewModel(IMedia media, IODataClient oDataClient, INavigationService navigationService
            , HttpClient httpClient, IClientAppProfile clientAppProfile, IUserDialogs userDialogs, IPageDialogService pageDialogService)
        {
            _oDataClinet = oDataClient;
            _navigationService = navigationService;
            _httpClient = httpClient;
            _clientAppProfile = clientAppProfile;
            _userDialogs = userDialogs;

            TakePhoto = new BitDelegateCommand<FileListViewItem>(async (parameter) =>
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
                     parameter.ItemSource = ImageSource;
                     parameter.File = Helpers.Helpers.ConvertMediaFileToBase64(file);
                 }

                 //parameter.IsVisible = true;
                 //int index = FileTypes.IndexOf(parameter);
                 //FileTypes.First().IsVisible = true;

                 //await CrossMedia.Current.Initialize();

                 //try
                 //{
                 //    Stream file = (await media.TakePhotoAsync(new StoreCameraMediaOptions { PhotoSize = PhotoSize.Small, AllowCropping = true, DefaultCamera = CameraDevice.Rear, RotateImage = true })).GetStream();
                 //    parameter.File = Helpers.Helpers.ConvertStreamToBase64(file);
                 //}
                 //catch { }
             });

            PickFromGallery = new BitDelegateCommand<FileListViewItem>(async (parameter) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await pageDialogService.DisplayAlertAsync("", "دسترسی به تصاویر وجود ندارد", "باشه");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    RotateImage = true,
                });

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
                    parameter.ItemSource = ImageSource;
                    parameter.File = Helpers.Helpers.ConvertMediaFileToBase64(file);
                }

            });

            Submit = new BitDelegateCommand(async () =>
            {

                bool confirmed = await pageDialogService.DisplayAlertAsync("مطمئن هستید؟", "درخواست ارسال شود؟", "بله", "خیر");

                if (confirmed)
                {
                    using (userDialogs.Loading(ConstantStrings.SendingRequest))
                    {
                        _httpClient.BaseAddress = new Uri($"{_clientAppProfile.HostUri}");
                        if (evlExpertRequestDto.Id == Guid.Empty)
                        {
                            string evlExpertJson = JsonConvert.SerializeObject(evlExpertRequestDto);
                            var stringContent = new StringContent(evlExpertJson, UnicodeEncoding.UTF8, "application/json");

                            evlExpertRequestDto = JsonConvert.DeserializeObject<EvlExpertRequestDto>(await (await _httpClient.PostAsync(_httpClient.BaseAddress + "api/EvlExpertRequests/SaveEvlExpert", stringContent)).Content.ReadAsStringAsync());

                            //evlExpertRequestDto = (await _oDataClinet.For<EvlExpertRequestDto>("EvlExpertRequests")
                            //.Set(evlExpertRequestDto)
                            //.InsertEntryAsync());
                        }
                    }

                    using (userDialogs.Loading(ConstantStrings.SendingPictures))
                    {
                        List<RequestFilesDto> requestFiles = new List<RequestFilesDto>();

                        foreach (var item in FileTypes.Where(f => f.File != null))
                        {
                            RequestFilesDto requestFile = new RequestFilesDto
                            {
                                EvlExpertRequestId = evlExpertRequestDto.Id,
                                File = item.File,
                                FileTypeId = item.FileType.Id,
                            };

                            var fileJson = JsonConvert.SerializeObject(requestFile);
                            var stringContent = new StringContent(fileJson, UnicodeEncoding.UTF8, "application/json");

                            requestFile = JsonConvert.DeserializeObject<RequestFilesDto>(await (await _httpClient.PostAsync(_httpClient.BaseAddress + "api/RequestFiles/SaveFile", stringContent)).Content.ReadAsStringAsync());

                        }

                        await navigationService.NavigateAsync("EvlExpertRequestWait");
                    }
                }
            });
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                parameters.Add("EvlExpertRequestDto", evlExpertRequestDto);
                base.OnNavigatedFrom(parameters);
            }
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            await CrossMedia.Current.Initialize();

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                parameters.TryGetValue("EvlExpertRequestDto", out evlExpertRequestDto);

                string path = _navigationService.GetNavigationUriPath();

                //var a = await _oDataClinet.For<FileTypeDto>("FileTypes")
                //          .FindEntriesAsync();

                FileTypeDto[] fileTypes = JsonConvert.DeserializeObject<FileTypeDto[]>(await _httpClient.GetStringAsync(_httpClient.BaseAddress + "api/FileTypes/GetAll"));

                FileTypes = new ObservableCollection<FileListViewItem>(fileTypes.Select(f => new FileListViewItem { FileType = f }));

                base.OnNavigatedTo(parameters);
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class FileListViewItem
    {
        public FileTypeDto FileType { get; set; } = new FileTypeDto();

        public ImageSource ItemSource { get; set; }

        public string File { get; set; }

        public bool IsVisible { get; set; }
    }
}
