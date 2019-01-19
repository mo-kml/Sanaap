using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestWaitViewModel : BitViewModelBase, IDestructible
    {
        private readonly IODataClient _odataClient;
        private EvlRequestItemSource _evlRequest;

        public string ExpertFullName { get; set; }
        public string ExpertMobileNo { get; set; }
        public ImageSource ExpertImage { get; set; }

        public BitDelegateCommand GoToMain { get; set; }
        public BitDelegateCommand Call { get; set; }
        public virtual BitDelegateCommand<int> RatingValueChanged { get; set; }

        public bool IsVisibleBefore { get; set; } = true;
        public bool IsVisibleAfter { get; set; } = false;

        public string Message { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IDateTimeUtils _dateTimeUtils;
        private readonly IEvlRequestService _evlRequestService;
        private IInitialDataService _initialDataService;
        private readonly HttpClient _httpClient;
        public EvlRequestWaitViewModel(INavigationService navigationService,
            IODataClient odataClient,
            IEvlRequestService evlRequestService,
            IDeviceService deviceService,
            IODataClient oDataClient,
            HttpClient httpClient,
            IDateTimeUtils dateTimeUtils,
            IInitialDataService initialDataService,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
            _evlRequestService = evlRequestService;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _initialDataService = initialDataService;
            _httpClient = httpClient;
            _dateTimeUtils = dateTimeUtils;

            GoToMain = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync($"/{nameof(MainView)}");
            });

            Call = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://" + ExpertMobileNo + ""));
            });

            //RatingValueChanged = new BitDelegateCommand<int>(async (value) =>
            //{
            //    await _pageDialogService.DisplayAlertAsync("", "RatingValueChanged : " + value.ToString(), ErrorMessages.Ok);
            //});
        }

        public override async Task OnNavigatingToAsync(INavigationParameters parameters)
        {
            _evlRequest = parameters.GetValue<EvlRequestItemSource>(nameof(EvlRequestItemSource));

            Message = ConstantStrings.ExpertFinding;
            IsVisibleBefore = true;
            IsVisibleAfter = false;
            string result = null;

            EvlRequestExpertDto expertDto = await _evlRequestService.FindEvlRequestExpert(_evlRequest.Id);

            try
            {
                CustomerDto customer = await _initialDataService.GetCurrentUserInfo();
                FindExpertRequestDto findExpertDto = new FindExpertRequestDto();
                findExpertDto.UserID = customer.Id.ToString();
                findExpertDto.RequestID = _evlRequest.Id.ToString();
                findExpertDto.Type = _evlRequest.InsuranceType == InsuranceType.Sales ? 2 : 1;
                findExpertDto.AccidentDate = _dateTimeUtils.ConvertMiladiToShamsi(DateTimeOffset.Now);
                findExpertDto.MapLat = _evlRequest.Latitude.ToString();
                findExpertDto.MapLng = _evlRequest.Longitude.ToString();
                findExpertDto.LostName = _evlRequest.LostFirstName;
                findExpertDto.LostFamily = _evlRequest.LostLastName;
                findExpertDto.LostInsuranceID = 1; // 1
                findExpertDto.LostCarID = _evlRequest.LostCarId;
                findExpertDto.LostCarType = "415"; // 415
                findExpertDto.Address = "یوسف آباد کوچه هفتم";
            }
            catch (Exception ex)
            {
                await _navigationService.NavigateAsync($"/{nameof(MainView)}");
                await _pageDialogService.DisplayAlertAsync("", ConstantStrings.FindNearExpertError, ErrorMessages.Ok);
            }

            if (result == "NotResult")
            {
                await _navigationService.NavigateAsync($"/{nameof(MainView)}");
                await _pageDialogService.DisplayAlertAsync("", ConstantStrings.FindNearExpertNotResult, ErrorMessages.Ok);
                return;
            }


            IsVisibleBefore = false;
            IsVisibleAfter = true;
            Message = ConstantStrings.ExpertFind;
            if (result != null)
            {
                string[] res = result.Split('^');
                ExpertFullName = res[0];
                ExpertMobileNo = res[1];
                byte[] imageAsBytes = Convert.FromBase64String(res[2].Split(',')[1]);
                ExpertImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            await base.OnNavigatingToAsync(parameters);
        }
    }
}
