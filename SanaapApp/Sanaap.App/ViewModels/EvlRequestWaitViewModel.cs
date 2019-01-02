using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestWaitViewModel : BitViewModelBase, IDestructible
    {
        private readonly IODataClient _odataClient;
        private EvlRequestDto _evlRequest;

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

        public EvlRequestWaitViewModel(INavigationService navigationService,
            IODataClient odataClient,
            IDeviceService deviceService,
            IODataClient oDataClient,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            GoToMain = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/Menu/Nav/Main");
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

        public override async Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            _evlRequest = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

            Message = ConstantStrings.ExpertFinding;
            IsVisibleBefore = true;
            IsVisibleAfter = false;

            string result = null;
            try
            {
                result = await _odataClient.For<string>("EvlRequestExperts")
                    .Function("FindEvlRequestExpert")
                    .Set(new { evlRequestId = _evlRequest.Id })
                    .FindEntryAsync();

            }
            catch (Exception ex)
            {
                await _navigationService.NavigateAsync("/Menu/Nav/Main");
                await _pageDialogService.DisplayAlertAsync("", ConstantStrings.FindNearExpertError, ErrorMessages.Ok);
            }

            if (result == "NotResult")
            {
                await _navigationService.NavigateAsync("/Menu/Nav/Main");
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
