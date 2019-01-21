using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Views;
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
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvaluationRequestFilesViewModel : BitViewModelBase
    {
        public BitDelegateCommand<EvlRequestFileItemSource> TakePhoto { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public ObservableCollection<EvlRequestFileItemSource> Files { get; set; }

        public EvlRequestItemSource Request { get; set; }

        private readonly IODataClient _oDataClient;
        private readonly IUserDialogs _userDialogs;
        public EvaluationRequestFilesViewModel(IMedia media,
            IODataClient oDataClient,

            IUserDialogs userDialogs,
            IPageDialogService pageDialogService,
            IPhotoHelper photoHelper,
            IClientAppProfile clientApp,
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

                MediaFile mediaFile = await photoHelper.TakePhoto();

                if (mediaFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        mediaFile.GetStream().CopyTo(ms);
                        file.Data = ms.ToArray();
                    }

                    file.Image = ImageSource.FromStream(() => mediaFile.GetStream());
                }
            });

            Submit = new BitDelegateCommand(async () =>
            {
                if (await pageDialogService.DisplayAlertAsync("مطمئن هستید؟", "درخواست ارسال شود؟", "بله", "خیر"))
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

                            await NavigationService.NavigateAsync(nameof(EvlRequestWaitView), new NavigationParameters
                            {
                                { nameof(EvlRequestItemSource), Request }
                            });
                        }

                    }
                }
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                Request = parameters.GetValue<EvlRequestItemSource>(nameof(EvlRequestItemSource));

                parameters.TryGetValue(nameof(Position), out Position position);

                Request.Latitude = position.Latitude;
                Request.Longitude = position.Longitude;


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

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            parameters.Add(nameof(EvlRequestItemSource), Request);
            parameters.Add(nameof(Position), new Position(Request.Latitude, Request.Longitude));
            parameters.Add("NextPage", "EvlRequestFile");
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}
