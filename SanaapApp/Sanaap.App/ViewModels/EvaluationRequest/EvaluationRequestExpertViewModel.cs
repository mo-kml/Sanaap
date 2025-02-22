﻿using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestExpertViewModel : BitViewModelBase
    {
        private readonly IEvlRequestService _evlRequestService;
        private readonly ILicenseHelper _licenseHelper;
        public EvaluationRequestExpertViewModel(IEvlRequestService evlRequestService, IPageDialogService dialogService, ILicenseHelper licenseHelper)
        {
            _evlRequestService = evlRequestService;
            _licenseHelper = licenseHelper;

            Call = new BitDelegateCommand(async () =>
            {
                Device.OpenUri(new Uri($"tel:{Expert.Expert.Mobile}"));
            });

            Cancel = new BitDelegateCommand(async () =>
            {
                if (await dialogService.DisplayAlertAsync(string.Empty, ConstantStrings.AreYouSureToCancel, ConstantStrings.Yes, ConstantStrings.No))
                {
                    await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMenuView)}");
                    return;
                }
            });
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Expert = parameters.GetValue<EvlRequestExpertDto>(nameof(Expert));
            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

            CurrentPosition = new Location { Latitude = Expert.Expert.MapLat, Longitude = Expert.Expert.MapLng };

            License = _licenseHelper.ConvertToItemSource(Expert.Expert.CarPlate);

            Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                Task.Run(async () =>
                {
                    ExpertPositionDto postion = await _evlRequestService.UpdateExpertPosition(Expert.Token);



                    CurrentPosition = new Location { Latitude = postion.Lat, Longitude = postion.Lng };
                });

                if (CurrentPosition.Latitude == 0 && CurrentPosition.Longitude == 0)
                {
                    NavigationService.NavigateAsync(nameof(EvaluationRequestExpertRankView), new NavigationParameters {
                            {nameof(Request),Request },
                            {nameof(Expert),Expert }
                        });

                    return false;
                }

                return true;
            });
        }

        public EvlRequestExpertDto Expert { get; set; }

        public EvlRequestItemSource Request { get; set; }

        public Location CurrentPosition { get; set; }

        public BitDelegateCommand Call { get; set; }

        public BitDelegateCommand Cancel { get; set; }

        public LicensePlateItemSource License { get; set; }
    }
}
