using Acr.UserDialogs;
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
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
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

        public EvlRequestItemSource Request { get; set; }

        private int _fileIndex;

        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        public EvaluationRequestFilesViewModel(IMedia media,
            IODataClient oDataClient,
            IUserDialogs userDialogs,
            IPageDialogService dialogService,
            IClientAppProfile clientApp,
            IEventAggregator eventAggregator,
            ISecurityService securityService)
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;

            TakePhoto = new BitDelegateCommand<EvlRequestFileItemSource>(async (file) =>
            {

                if (file.TypeName == "بیشتر")
                {
                    Files.Add(new EvlRequestFileItemSource { TypeId = file.TypeId, TypeName = file.TypeName, Image = file.Image });
                }

                _fileIndex = Files.IndexOf(file);

                eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());
            });

            OpenCamera = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());

                  MediaFile mediaFile = await media.TakePhotoAsync(new StoreCameraMediaOptions
                  {
                      Directory = "Sanaap",
                      SaveToAlbum = true,
                      CompressionQuality = 10,
                      CustomPhotoSize = 10,
                      PhotoSize = PhotoSize.Medium,
                      MaxWidthHeight = 2000,
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
                      await dialogService.DisplayAlertAsync("خطا", "تصویر انتخاب نشده است", "باشه");
                  }
              });

            Gallery = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<TakePhotoEvent>().Publish(new TakePhotoEvent());

                  MediaFile mediaFile = await media.PickPhotoAsync(new PickMediaOptions
                  {
                      PhotoSize = PhotoSize.Medium,
                      CompressionQuality = 10,
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
                      await dialogService.DisplayAlertAsync("خطا", "تصویر انتخاب نشده است", "باشه");
                  }
              });

            Submit = new BitDelegateCommand(async () =>
            {
                if (await dialogService.DisplayAlertAsync("مطمئن هستید؟", "درخواست ارسال شود؟", "بله", "خیر"))
                {
                    using (HttpClient http = new HttpClient())
                    {
                        http.BaseAddress = clientApp.HostUri;

                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await securityService.GetCurrentTokenAsync()).access_token);

                        using (userDialogs.Loading(ConstantStrings.SendingRequestAndPictures))
                        {
                            //EvlRequestDto evlRequest = new EvlRequestDto
                            //{
                            //    AccidentDate = Request.AccidentDate,
                            //    AccidentReason = Request.AccidentReason,
                            //    CarId = Request.CarId,
                            //    EvlRequestType = Request.EvlRequestType,
                            //    InsuranceType = Request.InsuranceType,
                            //    InsurerId = Request.InsurerId,
                            //    InsurerNo = Request.InsurerNo,
                            //    Latitude = Request.Latitude,
                            //    Longitude = Request.Longitude,
                            //    LostCarId = Request.LostCarId,
                            //    LostFirstName = Request.LostFirstName,
                            //    LostLastName = Request.LostLastName,
                            //    LostPlateNumber = Request.LostPlateNumber,
                            //    OwnerFirstName = Request.OwnerFirstName,
                            //    OwnerLastName = Request.OwnerLastName,
                            //    PlateNumber = Request.PlateNumber,
                            //    Status = Request.Status
                            //};

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
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                using (_userDialogs.Loading(ConstantStrings.Loading))
                {
                    Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

                    List<ExternalEntityDto> ImageFileTypes = new List<ExternalEntityDto>
                {
                    new ExternalEntityDto{Name="کارت ملی", PrmID=52},
                    new ExternalEntityDto{Name="عکس خودرو", PrmID=56},
                    new ExternalEntityDto{Name="بیمه نامه ثالث", PrmID=58},
                    new ExternalEntityDto{Name="شناسنامه", PrmID=57},
                    new ExternalEntityDto{Name="بیشتر", PrmID=59},
                };

                    List<EvlRequestFileItemSource> files = ImageFileTypes.Select(i => new EvlRequestFileItemSource { TypeId = i.PrmID, TypeName = i.Name, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") }).ToList();

                    Files = new ObservableCollection<EvlRequestFileItemSource>(files);

                    await base.OnNavigatedToAsync(parameters);
                }
            }
        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            //parameters.Add(nameof(EvlRequestItemSource), Request);
            //parameters.Add(nameof(Position), new Position(Request.Latitude, Request.Longitude));
            //parameters.Add("NextPage", "EvlRequestFile");
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
