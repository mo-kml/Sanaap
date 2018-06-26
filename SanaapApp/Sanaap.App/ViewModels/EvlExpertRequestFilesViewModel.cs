using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Sanaap.App.Dto;
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
        public EvlExpertRequestFilesViewModel(IMedia media, IODataClient oDataClient, INavigationService navigationService, HttpClient httpClient, IClientAppProfile clientAppProfile)
        {
            _oDataClinet = oDataClient;
            _navigationService = navigationService;
            _httpClient = httpClient;
            _clientAppProfile = clientAppProfile;

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
                  if (evlExpertRequestDto.Id == Guid.Empty)
                  {
                      _httpClient.BaseAddress = new Uri($"{_clientAppProfile.HostUri}api/EvlExpertRequests/SaveEvlExpert");
                      var myContent = JsonConvert.SerializeObject(evlExpertRequestDto);
                      var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");

                      var a = await _httpClient.PostAsync(_httpClient.BaseAddress, stringContent);
                      //evlExpertRequestDto = (await _oDataClinet.For<EvlExpertRequestDto>("EvlExpertRequests")
                      //.Set(evlExpertRequestDto)
                      //.InsertEntryAsync());
                  }
                  List<RequestFilesDto> requestFiles = new List<RequestFilesDto>();

                  foreach (var item in FileTypes.Where(f => f.File != null))
                  {
                      RequestFilesDto requestFile = new RequestFilesDto
                      {
                          EvlExpertRequestId = evlExpertRequestDto.Id,
                          File = item.File,
                          FileTypeId = item.FileType.Id,
                      };

                      requestFile = (await _oDataClinet.For<RequestFilesDto>("RequestFiles")
                      .Set(requestFile)
                      .InsertEntryAsync());
                  }
              });
        }
        public ObservableCollection<FileListViewItem> FileTypes { get; set; }

        public BitDelegateCommand<FileListViewItem> TakePhoto { get; set; }

        public BitDelegateCommand<FileListViewItem> PickFromGallery { get; set; }

        public BitDelegateCommand Submit { get; set; }
        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            parameters.Add("EvlExpertRequestDto", evlExpertRequestDto);

            base.OnNavigatedFrom(parameters);
        }
        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("EvlExpertRequestDto", out evlExpertRequestDto);

            string path = _navigationService.GetNavigationUriPath();

            //var a = await _oDataClinet.For<FileTypeDto>("FileTypes")
            //          .FindEntriesAsync();

            List<FileListViewItem> files = new List<FileListViewItem>
            {

                new FileListViewItem{ FileType=new FileTypeDto { Id = Guid.NewGuid(), Name = "کد ملی" } },
                new FileListViewItem{FileType=new FileTypeDto { Id=Guid.NewGuid(), Name="شناسنامه"} },
                new FileListViewItem{FileType=new FileTypeDto { Id=Guid.NewGuid(), Name="کد ملی" } },
                new FileListViewItem{FileType=new FileTypeDto { Id=Guid.NewGuid(), Name="کد ملی"} },
            };

            FileTypes = new ObservableCollection<FileListViewItem>(files);

            base.OnNavigatedTo(parameters);
        }
    }

    public class FileListViewItem
    {
        public FileTypeDto FileType { get; set; } = new FileTypeDto();

        public ImageSource ItemSource { get; set; }

        public string File { get; set; }
    }
}
