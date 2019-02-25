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
using System.Linq;
using System.Threading;
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
                inquiryCancellationTokenSource?.Cancel();
                inquiryCancellationTokenSource = new CancellationTokenSource();
                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: inquiryCancellationTokenSource.Cancel))
                {
                    if (string.IsNullOrEmpty(DocumentNumber) || !Requests.Any(r => r.Code == int.Parse(DocumentNumber)))
                    {
                        await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.DocumentNumberIsInvalid, ConstantStrings.Ok);
                        DocumentNumber = null;
                        return;
                    }

                    EvlRequestDto requestDto = await evlRequestService.SearchByCode(int.Parse(DocumentNumber));

                    if (requestDto == null)
                    {
                        await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.RequestDosentExist, ConstantStrings.Ok);
                        DocumentNumber = null;
                        return;
                    }
                    else
                    {
                        DocumentNumber = null;

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
            });
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            inquiryCancellationTokenSource?.Cancel();
            inquiryCancellationTokenSource = new CancellationTokenSource();
            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: inquiryCancellationTokenSource.Cancel))
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

        private CancellationTokenSource inquiryCancellationTokenSource;

        public string DocumentNumber { get; set; }

        public BitDelegateCommand<EvlRequestListItemSource> ShowRequestProgress { get; set; }

        public bool IsSelected { get; set; }
    }
}
