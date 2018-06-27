using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestFilesViewModel : BitViewModelBase
    {
        private EvlExpertRequestDto evlExpertRequestDto;
        private readonly IODataClient _oDataClinet;
        private readonly INavigationService _navigationService;
        private readonly HttpClient _httpClient;
        private readonly IClientAppProfile _clientAppProfile;
        private readonly IUserDialogs _userDialogs;

        public EvlExpertRequestFilesViewModel(IMedia media, IODataClient oDataClient, INavigationService navigationService
            , HttpClient httpClient, IClientAppProfile clientAppProfile, IUserDialogs userDialogs)
        {
            _oDataClinet = oDataClient;
            _navigationService = navigationService;
            _httpClient = httpClient;
            _clientAppProfile = clientAppProfile;
            _userDialogs = userDialogs;

            TakePhoto = new BitDelegateCommand<FileListViewItem>(async (parameter) =>
             {
                 Stream file = (await media.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true, DefaultCamera = CameraDevice.Rear, RotateImage = true })).GetStream();

                 parameter.File = Helpers.Helpers.ConvertStreamToBase64(file);
             });

            PickFromGallery = new BitDelegateCommand<FileListViewItem>(async (parameter) =>
            {
                Stream file = (await media.PickPhotoAsync(new PickMediaOptions { RotateImage = true })).GetStream();

                parameter.File = Helpers.Helpers.ConvertStreamToBase64(file);
            });

            Submit = new BitDelegateCommand(async () =>
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

                    await navigationService.NavigateAsync("/EvlExpertRequestWait");
                }
            });
        }

        public ObservableCollection<FileListViewItem> FileTypes { get; set; }

        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }

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

    public class FileListViewItem
    {
        public FileTypeDto FileType { get; set; } = new FileTypeDto();

        public ImageSource ItemSource { get; set; }

        public string File { get; set; }
    }
}
