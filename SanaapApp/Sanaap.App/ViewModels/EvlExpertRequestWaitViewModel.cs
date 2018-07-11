using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Sanaap.Enums.Enums;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestWaitViewModel : BitViewModelBase, IDestructible, IDisposable
    {
        private readonly HttpClient _httpClient;
        //private readonly HttpClient _httpClientLogin;
        //private readonly HttpClient _httpClientNearestExpert;
        private readonly ODataClient _oDataClient;
        private readonly IClientAppProfile _clientAppProfile;
        private readonly IODataClient _odataClient;

        private CustomerDto customerDto;

        private EvlExpertRequestDto evlExpertRequestDto;

        public string FullName { get; set; }
        public string Mobile { get; set; }
        public ImageSource ImageSource { get; set; }

        public BitDelegateCommand GoToMain { get; set; }
        public BitDelegateCommand Call { get; set; }

        public bool IsVisibleBefore { get; set; } = true;
        public bool IsVisibleAfter { get; set; } = false;

        public string Message { get; set; }

        public EvlExpertRequestWaitViewModel(INavigationService navigationService, HttpClient httpClient
            , IClientAppProfile clientAppProfile, IODataClient odataClient, IDeviceService deviceService, IODataClient oDataClient)
        {
            _httpClient = httpClient;
            //_httpClientLogin = httpClient;
            //_httpClientNearestExpert = httpClient;
            _clientAppProfile = clientAppProfile;
            _odataClient = odataClient;

            GoToMain = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/Menu/Nav/Main");
                this.Destroy();
                Dispose();
            });

            Call = new BitDelegateCommand(() =>
            {
                deviceService.OpenUri(new Uri("tel://" + Mobile + ""));
            });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            Message = ConstantStrings.ExpertFinding;
            parameters.TryGetValue("EvlExpertRequestDto", out evlExpertRequestDto);
            SoltaniFindNearExpert expert = await GetExpert();
            Message = ConstantStrings.ExpertFind;
            IsVisibleBefore = false;
            IsVisibleAfter = true;
            FullName = expert.ExpName.Trim();
            Mobile = expert.ExpMobile.Trim();
            byte[] imageAsBytes = Convert.FromBase64String(expert.ExpPhoto.Split(',')[1]);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));

            //UpdateRequestStatus(evlExpertRequestDto.Id, EnumRequestStatus.TaeenKarshenas);

            base.OnNavigatedTo(parameters);
        }

        public async Task<SoltaniFindNearExpert> GetExpert()
        {
            _httpClient.BaseAddress = new Uri(ConstantStrings.SoltaniApis);
            SoltaniLoginParams soltaniLoginParams = new SoltaniLoginParams();
            string soltaniLoginParamsJson = JsonConvert.SerializeObject(soltaniLoginParams);
            var stringContent = new StringContent(soltaniLoginParamsJson, UnicodeEncoding.UTF8, "application/json");
            SoltaniLogin soltaniLogin = JsonConvert.DeserializeObject<SoltaniLogin>(
                await (await _httpClient.PostAsync(_httpClient.BaseAddress + "api/Portal/Login"
                , stringContent)).Content.ReadAsStringAsync());

            customerDto = await _odataClient.For<CustomerDto>("Customers").Function("GetCurrentCustomer").ExecuteAsSingleAsync();
            HttpClient httpClientFindNearExpert = new HttpClient();
            httpClientFindNearExpert.DefaultRequestHeaders.Add("auth", soltaniLogin.token);
            SoltaniFindNearExpertParams soltaniFindNearExpertParams = new SoltaniFindNearExpertParams();
            soltaniFindNearExpertParams.UserID = evlExpertRequestDto.CustomerId.ToString();
            soltaniFindNearExpertParams.RequestID = evlExpertRequestDto.Id.ToString();
            soltaniFindNearExpertParams.Type = evlExpertRequestDto.InsuranceTypeEnum == Enums.Enums.InsuranceTypeEnum.Sales ? "1" : "2";
            soltaniFindNearExpertParams.MapLat = evlExpertRequestDto.Latitude.ToString();
            soltaniFindNearExpertParams.MapLng = evlExpertRequestDto.Longitude.ToString();
            soltaniFindNearExpertParams.LostName = evlExpertRequestDto.OwnerFullName != null ? evlExpertRequestDto.OwnerFullName.Trim() : customerDto.FirstName;
            soltaniFindNearExpertParams.LostFamily = evlExpertRequestDto.OwnerFullName != null ? "" : customerDto.LastName;
            soltaniFindNearExpertParams.LostMobile = evlExpertRequestDto.OwnerMobileNumber != null ? evlExpertRequestDto.OwnerMobileNumber : customerDto.Mobile;
            soltaniFindNearExpertParams.LostInsuranceID = "0";
            soltaniFindNearExpertParams.LostInsuranceNO = evlExpertRequestDto.InsuranceNumber;
            soltaniFindNearExpertParams.LostCarID = "12608";
            soltaniFindNearExpertParams.LostCarType = "415";
            string soltaniFindNearExpertParamsJson = JsonConvert.SerializeObject(soltaniFindNearExpertParams);
            var stringContentNearExpert = new StringContent(soltaniFindNearExpertParamsJson, UnicodeEncoding.UTF8, "application/json");
            SoltaniFindNearExpert soltaniFindNearExpert = JsonConvert.DeserializeObject<SoltaniFindNearExpert>(
                await (await httpClientFindNearExpert.PostAsync(_httpClient.BaseAddress + "api/Portal/FindNearExpert"
                , stringContentNearExpert)).Content.ReadAsStringAsync());
            httpClientFindNearExpert.Dispose();
            //_httpClient.Dispose();
            return soltaniFindNearExpert;
        }

        public async void UpdateRequestStatus(Guid evlExpertRequestId, EnumRequestStatus enumRequestStatus)
        {
            //await _oDataClient.For<EvlExpertRequestDto>("Customers")
            //                    .Action("RegisterCustomer")
            //                    .Set(new
            //                    {
            //                        customer = Customer
            //                    })
            //                    .ExecuteAsync();

            _httpClient.BaseAddress = new Uri($"{_clientAppProfile.HostUri}");

            string evlExpertJson = JsonConvert.SerializeObject(new EvlExpertRequestUpdateStatusArgs { EvlExpertRequestId = evlExpertRequestDto.Id, EnumRequestStatus = EnumRequestStatus.TaeenKarshenas });
            var stringContent = new StringContent(evlExpertJson, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync(_httpClient.BaseAddress + "api/EvlExpertRequests/UpdateRequestStatus", stringContent);
        }

        public void Dispose()
        {
            this.Destroy();
        }
    }

    public class EvlExpertRequestUpdateStatusArgs
    {
        public Guid EvlExpertRequestId { get; set; }

        public EnumRequestStatus EnumRequestStatus { get; set; }
    }
}
