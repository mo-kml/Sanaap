using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestListViewModel : BitViewModelBase
    {
        private IEvlRequestService _evlRequestService;
        private IDateTimeUtils _dateTimeUtils;
        private readonly IUserDialogs _userDialogs;
        private readonly IPageDialogService _dialogService;
        public EvaluationRequestListViewModel(IEvlRequestService evlRequestService, IDateTimeUtils dateTimeUtils, IUserDialogs userDialogs, IPageDialogService dialogService)
        {
            _evlRequestService = evlRequestService;
            _dateTimeUtils = dateTimeUtils;
            _userDialogs = userDialogs;
            _dialogService = dialogService;

            ShowRequestProgress = new BitDelegateCommand<EvlRequestListItemSource>(async (request) =>
            {
                INavigationParameters parameters = new NavigationParameters();
                parameters.Add(nameof(EvlRequestListItemSource), request);

                await NavigationService.NavigateAsync(nameof(EvlRequestProgressView), parameters);
            });

            Inquiry = new BitDelegateCommand(async () =>
            {
                using (userDialogs.Loading(ConstantStrings.Loading))
                {
                    EvlRequestDto requestDto = await evlRequestService.SearchByCode(DocumentNumber);

                    if (requestDto == null)
                    {
                        await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.RequestDosentExist, ConstantStrings.Ok);
                        DocumentNumber = 0;
                        return;
                    }
                    else
                    {
                        INavigationParameters parameter = new NavigationParameters();
                        parameter.Add(nameof(EvlRequestListItemSource), new EvlRequestListItemSource
                        {
                            Code = requestDto.Code,
                            RequestId = requestDto.Id,
                            RequestTypeName = EnumHelper<EvlRequestType>.GetDisplayValue(requestDto.EvlRequestType)
                        });

                        await NavigationService.NavigateAsync(nameof(EvlRequestProgressView), parameter);
                    }
                }
            }, () => DocumentNumber != 0);
            Inquiry.ObservesProperty(() => DocumentNumber);
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                //await loadRequests();
            }
        }

        public async Task loadRequests()
        {
            IEnumerable<EvlRequestDto> requests = await _evlRequestService.GetAllRequests();

            Requests = new ObservableCollection<EvlRequestListItemSource>();

            foreach (EvlRequestDto r in requests)
            {
                Requests.Add(new EvlRequestListItemSource
                {
                    Code = r.Code,
                    RequestTypeName = EnumHelper<EvlRequestType>.GetDisplayValue(r.EvlRequestType),
                    Date = _dateTimeUtils.ConvertMiladiToShamsi(r.CreatedOn),
                    RequestId = r.Id
                });
            }
        }
        public ObservableCollection<EvlRequestListItemSource> Requests { get; set; }

        public BitDelegateCommand Inquiry { get; set; }

        public int DocumentNumber { get; set; }

        public BitDelegateCommand<EvlRequestListItemSource> ShowRequestProgress { get; set; }
    }
}
