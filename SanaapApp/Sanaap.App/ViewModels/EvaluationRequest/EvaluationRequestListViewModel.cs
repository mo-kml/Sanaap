using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestListViewModel : BitViewModelBase
    {
        private IEvlRequestService _evlRequestService;
        private readonly IUserDialogs _userDialogs;
        private readonly IPageDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        public EvaluationRequestListViewModel(IEvlRequestService evlRequestService, IUserDialogs userDialogs, IPageDialogService dialogService, IEventAggregator eventAggregator)
        {
            _evlRequestService = evlRequestService;
            _userDialogs = userDialogs;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            ShowRequestProgress = new BitDelegateCommand<EvlRequestListItemSource>(async (request) =>
            {
                await NavigationService.NavigateAsync(nameof(EvlRequestProgressView), new NavigationParameters
                {
                    {nameof(EvlRequestListItemSource),request }
                });
            });

            OpenInquiryBox = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<OpenInquiryPopupEvent>().Publish(new OpenInquiryPopupEvent());
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
                        DocumentNumber = 0;

                        eventAggregator.GetEvent<OpenInquiryPopupEvent>().Publish(new OpenInquiryPopupEvent());

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
                await loadRequests();
            }
        }

        public async Task loadRequests()
        {
            Requests = new ObservableCollection<EvlRequestListItemSource>(await _evlRequestService.GetAllRequests());
        }
        public ObservableCollection<EvlRequestListItemSource> Requests { get; set; }

        public BitDelegateCommand Inquiry { get; set; }

        public BitDelegateCommand OpenInquiryBox { get; set; }

        public int DocumentNumber { get; set; }

        public BitDelegateCommand<EvlRequestListItemSource> ShowRequestProgress { get; set; }

        public bool IsSelected { get; set; }
    }
}
