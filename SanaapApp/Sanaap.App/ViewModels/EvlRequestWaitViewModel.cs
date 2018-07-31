using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
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
        }

        public async override Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            _evlRequest = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

            Message = ConstantStrings.ExpertFinding;
            IsVisibleBefore = true;
            IsVisibleAfter = false;

            EvlRequestExpertDto evlRequestExpert = null;
            try
            {
                evlRequestExpert = await _odataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
                    .Function("FindEvlRequestExpert")
                    .Set(new { evlRequestId = _evlRequest.Id })
                    .FindEntryAsync();
            }
            catch (Exception ex)
            {
                await _navigationService.NavigateAsync("/Menu/Nav/Main");
                await _pageDialogService.DisplayAlertAsync("", ConstantStrings.FindNearExpertError, ErrorMessages.Ok);
            }


            if (evlRequestExpert != null)
            {
                IsVisibleBefore = false;
                IsVisibleAfter = true;

                Message = ConstantStrings.ExpertFind;
                ExpertFullName = evlRequestExpert.ExpName.Trim();
                ExpertMobileNo = evlRequestExpert.ExpMobile.Trim();
                byte[] imageAsBytes = Convert.FromBase64String(evlRequestExpert.ExpPhoto.Split(',')[1]);
                ExpertImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            await base.OnNavigatingToAsync(parameters);
        }
    }
}
