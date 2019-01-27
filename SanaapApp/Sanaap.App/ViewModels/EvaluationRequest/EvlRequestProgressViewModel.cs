using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Sanaap.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvlRequestProgressViewModel : BitViewModelBase
    {
        private readonly IEvlRequestService _evlRequestService;
        private readonly IUserDialogs _userDialogs;
        private readonly IDateHelper _dateHelper;
        public EvlRequestProgressViewModel(IEvlRequestService evlRequestService, IUserDialogs userDialogs, IDateHelper dateHelper)
        {
            _evlRequestService = evlRequestService;
            _userDialogs = userDialogs;
            _dateHelper = dateHelper;
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            //parameters.TryGetValue(nameof(EvlRequestListItemSource), out EvlRequestListItemSource request);

            //RequestCode = request.Code;

            //using (_userDialogs.Loading(ConstantStrings.Loading))
            //{
            //    await loadProgresses(request.RequestId);
            //}
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
                    Date = _dateHelper.ToPersianShortDate(progress.CreatedOn.Date),
                    Status = EnumHelper<EvlRequestStatus>.GetDisplayValue(progress.EvlRequestStatus)
                });
            }
        }
    }
}
