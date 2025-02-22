﻿using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Controls;
using Sanaap.App.Dto;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestFilesViewModel : BitViewModelBase
    {
        public BitDelegateCommand<EvlRequestFileItemSource> TakePhoto { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand<EvlRequestFileItemSource> OpenPhoto { get; set; }

        public ObservableCollection<EvlRequestFileItemSource> Files { get; set; }

        public BitDelegateCommand OpenCamera { get; set; }

        public BitDelegateCommand Gallery { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public BitDelegateCommand<EvlRequestFileItemSource> DeletePhoto { get; set; }

        public EvlRequestItemSource Request { get; set; }

        private int _fileIndex;

        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        private readonly IInitialDataService _initialDataService;
        public EvaluationRequestFilesViewModel(IMedia media,
            IODataClient oDataClient,
            IUserDialogs userDialogs,
            IInitialDataService initialDataService,
            IPageDialogService dialogService,
            IClientAppProfile clientApp,
            IEventAggregator eventAggregator,
            ISecurityService securityService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _initialDataService = initialDataService;

            TakePhoto = new BitDelegateCommand<EvlRequestFileItemSource>(async (file) =>
            {
                //500 is ID for More pictures
                if (file.TypeId == 500)
                {
                    Files.Add(new EvlRequestFileItemSource { TypeId = file.TypeId, TypeName = file.TypeName, Image = file.Image });
                }

                _fileIndex = Files.IndexOf(file);

                eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());
            });

            GoBack = new BitDelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            OpenCamera = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());

                  MediaFile mediaFile = await media.TakePhotoAsync(new StoreCameraMediaOptions
                  {
                      Directory = "Sanaap",
                      SaveToAlbum = true,
                      CompressionQuality = 25,
                      CustomPhotoSize = 30,
                      PhotoSize = PhotoSize.Medium,
                      MaxWidthHeight = 2000,
                      AllowCropping = true,
                      DefaultCamera = CameraDevice.Rear
                  });

                  if (mediaFile != null)
                  {
                      using (MemoryStream ms = new MemoryStream())
                      {
                          mediaFile.GetStream().CopyTo(ms);
                          Files[_fileIndex].Data = ms.ToArray();
                      }

                      Files[_fileIndex].Image = ImageSource.FromStream(() => mediaFile.GetStream());
                      Files[_fileIndex].HasImage = true;
                  }
                  else
                  {
                      await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.PictureIsNull, ConstantStrings.Ok);
                  }
              });

            Gallery = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());

                  MediaFile mediaFile = await media.PickPhotoAsync(new PickMediaOptions
                  {
                      PhotoSize = PhotoSize.Medium,
                      CompressionQuality = 25,
                  });

                  if (mediaFile != null)
                  {
                      using (MemoryStream ms = new MemoryStream())
                      {
                          mediaFile.GetStream().CopyTo(ms);
                          Files[_fileIndex].Data = ms.ToArray();
                      }

                      Files[_fileIndex].Image = ImageSource.FromStream(() => mediaFile.GetStream());
                      Files[_fileIndex].HasImage = true;
                  }
                  else
                  {
                      await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.PictureIsNull, ConstantStrings.Ok);
                  }
              });

            Submit = new BitDelegateCommand(async () =>
            {
                if (Files.Any(f => f.IsRequired && f.HasImage == false))
                {
                    await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.PictureIsRequired, ConstantStrings.Ok);

                    return;
                }

                if (await dialogService.DisplayAlertAsync(string.Empty, ConstantStrings.AreYouSure, ConstantStrings.Yes, ConstantStrings.No))
                {
                    using (HttpClient http = new HttpClient())
                    {
                        http.BaseAddress = clientApp.HostUri;

                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await securityService.GetCurrentTokenAsync()).access_token);

                        using (userDialogs.Loading(ConstantStrings.SendingRequestAndPictures))
                        {
                            MultipartFormDataContent submitEvlRequestContents = new MultipartFormDataContent
                            {
                                new StringContent(JsonConvert.SerializeObject(Request), Encoding.UTF8, "application/json")
                            };

                            foreach (EvlRequestFileDto file in Files.Where(f => f.Data != null))
                            {
                                submitEvlRequestContents.Add(new ByteArrayContent(file.Data), file.TypeId.ToString(), file.TypeId.ToString());
                            }

                            HttpRequestMessage submitEvlRequest = new HttpRequestMessage(HttpMethod.Post, "api/evl-requests/submit-evl-request")
                            {
                                Content = submitEvlRequestContents
                            };

                            HttpResponseMessage submitEvlRequestExpertResponse = await http.SendAsync(submitEvlRequest);

                            submitEvlRequestExpertResponse.EnsureSuccessStatusCode();

                            Request = JsonConvert.DeserializeObject<EvlRequestItemSource>(await submitEvlRequestExpertResponse.Content.ReadAsStringAsync());

                            await NavigationService.NavigateAsync(nameof(EvaluationRequestWaitView), new NavigationParameters
                            {
                                { nameof(Request), Request }
                            });
                        }

                    }
                }
            });

            OpenPhoto = new BitDelegateCommand<EvlRequestFileItemSource>(async (image) =>
              {
                  await NavigationService.NavigateAsync(nameof(OpenImagePopup), new NavigationParameters
                  {
                      {nameof(image.Image), image.Image}
                  });
              });

            DeletePhoto = new BitDelegateCommand<EvlRequestFileItemSource>(async (image) =>
            {
                if (image.HasImage == false)
                {
                    return;
                }

                if (await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.AreYouSureToDelete, ConstantStrings.Yes, ConstantStrings.No))
                {
                    image.Image = null;
                    image.HasImage = false;

                    await NavigationService.GoBackAsync();
                }
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                using (_userDialogs.Loading(ConstantStrings.Loading))
                {
                    Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

                    List<PhotoTypeDto> photos;

                    if (Request.InsuranceType == InsuranceType.Sales)
                    {
                        photos = await _initialDataService.GetSalesPhotos();
                    }
                    else
                    {
                        photos = await _initialDataService.GetBadanePhotos();
                    }

                    List<EvlRequestFileItemSource> files = photos.Select(i =>
                    new EvlRequestFileItemSource
                    {
                        IsRequired = i.IsRequired,
                        TypeId = i.ID,
                        TypeName = i.Name,
                        Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg")
                    }).ToList();

                    Files = new ObservableCollection<EvlRequestFileItemSource>(files);

                    await base.OnNavigatedToAsync(parameters);
                }
            }

        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            parameters.Add(nameof(Request), Request);
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
