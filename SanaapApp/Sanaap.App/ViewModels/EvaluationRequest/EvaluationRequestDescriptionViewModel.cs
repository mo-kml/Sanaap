using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Service.Contracts;
using System;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestDescriptionViewModel : BitViewModelBase
    {
        private readonly IDateHelper _dateHelper;
        private readonly IPageDialogService _dialogService;
        public EvaluationRequestDescriptionViewModel(
            IDateHelper dateHelper,
            ISanaapAppTranslateService translateService,
            IPageDialogService dialogService,
            IEvlRequestValidator evlRequestValidator)
        {
            _dateHelper = dateHelper;
            _dialogService = dialogService;

            GoToNextLevel = new BitDelegateCommand(async () =>
              {
                  Request.AccidentDate = new DateTimeOffset((DateTime)SelectedDate, TimeSpan.Zero);

                  if (!evlRequestValidator.IsDescriptionValid(Request, out string message))
                  {
                      await dialogService.DisplayAlertAsync(string.Empty, translateService.Translate(message), ConstantStrings.Ok);
                      return;
                  }

                  await NavigationService.NavigateAsync(nameof(EvaluationRequestMapView), new NavigationParameters
                  {
                      {nameof(Request),Request }
                  });
              }, () => SelectedDate != null);
            GoToNextLevel.ObservesProperty(() => SelectedDate);

            GoBack = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync();
              });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

            if (Request.AccidentDate != new DateTimeOffset())
            {
                //SelectedDate = DateTime.Now;
            }
        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            parameters.Add(nameof(Request), Request);
            return base.OnNavigatedFromAsync(parameters);
        }
        public string Month { get; set; }

        public string Year { get; set; }

        public string Day { get; set; }

        public DateTime? SelectedDate { get; set; }

        public EvlRequestItemSource Request { get; set; }

        public BitDelegateCommand GoToNextLevel { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public void OnSelectedDateChanged()
        {
            if (SelectedDate.Value.Date > DateTime.Now)
            {
                SelectedDate = DateTime.Now;
            }

            _dateHelper.ToPersianLongDate(SelectedDate.Value, out string year, out string month, out string day);

            Year = year;
            Month = month;
            Day = day;
        }
    }
}
