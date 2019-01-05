using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
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
    public class EvlRequestProgressViewModel : BitViewModelBase
    {
        private readonly IEvlRequestService _evlRequestService;
        private readonly IDateTimeUtils _dateTimeUtils;
        private IUserDialogs _userDialogs;
        public EvlRequestProgressViewModel(IEvlRequestService evlRequestService, IDateTimeUtils dateTimeUtils, IUserDialogs userDialogs)
        {
            _evlRequestService = evlRequestService;
            _dateTimeUtils = dateTimeUtils;
            _userDialogs = userDialogs;
        }
        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            parameters.TryGetValue(nameof(EvlRequestListItemSource), out EvlRequestListItemSource request);

            RequestCode = request.Code;

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                await loadProgresses(request.RequestId);
            }
        }

        public ObservableCollection<ProgressItemSource> Progresses { get; set; }

        public long RequestCode { get; set; }

        public async Task loadProgresses(Guid requestId)
        {
            IEnumerable<EvlRequestProgressDto> progresses = await _evlRequestService.GetAllProgressesByRequestId(requestId);

            Progresses = new ObservableCollection<ProgressItemSource>();

            foreach (EvlRequestProgressDto progress in progresses)
            {
                Progresses.Add(new ProgressItemSource
                {
                    Date = _dateTimeUtils.ConvertMiladiToShamsi(progress.CreatedOn),
                    Status = EnumHelper<EvlRequestStatus>.GetDisplayValue(progress.EvlRequestStatus)
                });
            }
        }
    }
}
