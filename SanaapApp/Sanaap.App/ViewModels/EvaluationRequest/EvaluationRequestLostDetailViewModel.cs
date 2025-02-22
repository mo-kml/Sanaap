﻿using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestLostDetailViewModel : BitViewModelBase
    {
        private IInitialDataService _initialDataService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILicenseHelper _licenseHelper;
        public EvaluationRequestLostDetailViewModel(
            IUserDialogs userDialogs,
            ILicenseHelper licenseHelper,
            ISanaapAppTranslateService translateService,
            IEvlRequestValidator evlRequestValidator,
            IInitialDataService initialDataService,
            IPageDialogService dialogService)
        {
            _initialDataService = initialDataService;
            _userDialogs = userDialogs;
            _licenseHelper = licenseHelper;

            GoBack = new BitDelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            GoToNextLevel = new BitDelegateCommand(async () =>
            {
                requestCancellationTokenSource?.Cancel();
                requestCancellationTokenSource = new CancellationTokenSource();

                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: requestCancellationTokenSource.Cancel))
                {
                    if (SelectedCar == null)
                    {
                        await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.CarIsNull, ConstantStrings.Ok);
                        return;
                    }
                    if (SelectedAlphabet == null)
                    {
                        await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.NumberPlateIsNotValid, ConstantStrings.Ok);
                        return;
                    }

                    Request.LostCarId = SelectedCar.PrmID;

                    LostLicense.Alphabet = SelectedAlphabet.Name;
                    if (licenseHelper.ConvertToPlateNumber(LostLicense, out string licensePlate))
                    {
                        Request.LostPlateNumber = licensePlate;
                    }
                    else
                    {
                        return;
                    }

                    if (!evlRequestValidator.IsLostDetailValid(Request, out string message))
                    {
                        await dialogService.DisplayAlertAsync(string.Empty, translateService.Translate(message), ConstantStrings.Ok);
                        return;
                    }

                    await NavigationService.NavigateAsync(nameof(EvaluationRequestDescriptionView), new NavigationParameters
                    {
                        { nameof(Request),Request}
                    });
                }
            });
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                await syncData();
            }

            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

            SelectedCar = Request.LostCarId != 0 ? Cars.FirstOrDefault(c => c.PrmID == Request.LostCarId) : new ExternalEntityDto();
            LostLicense = !string.IsNullOrEmpty(Request.LostPlateNumber) ? _licenseHelper.ConvertToItemSource(Request.LostPlateNumber) : new LicensePlateItemSource();
            SelectedAlphabet = LostLicense != null ? Alphabets.FirstOrDefault(c => c.Name == LostLicense.Alphabet) : new ExternalEntityDto();
        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            parameters.Add(nameof(Request), Request);
            return base.OnNavigatedFromAsync(parameters);
        }

        public async Task syncData()
        {
            Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

            Alphabets = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetAlphabets());
        }

        public EvlRequestItemSource Request { get; set; }

        public ObservableCollection<ExternalEntityDto> Alphabets { get; set; }

        public ExternalEntityDto SelectedAlphabet { get; set; }

        public LicensePlateItemSource LostLicense { get; set; } = new LicensePlateItemSource();

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }

        public BitDelegateCommand GoToNextLevel { get; set; }

        public CancellationTokenSource requestCancellationTokenSource { get; set; }

    }
}
