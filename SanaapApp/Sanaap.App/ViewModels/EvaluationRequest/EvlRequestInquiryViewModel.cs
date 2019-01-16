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
using System;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvlRequestInquiryViewModel : BitViewModelBase
    {

        public EvlRequestInquiryViewModel(IEvlRequestService evlRequestService, IPageDialogService dialogService, INavigationService navigationService, IUserDialogs userDialogs)
        {

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
                          NavigationParameters parameter = new NavigationParameters();
                          parameter.Add(nameof(EvlRequestListItemSource), new EvlRequestListItemSource
                          {
                              Code = requestDto.Code,
                              RequestId = requestDto.Id,
                              RequestTypeName = EnumHelper<EvlRequestType>.GetDisplayValue(requestDto.EvlRequestType)
                          });

                          await navigationService.NavigateAsync(nameof(EvlRequestProgressView), parameter);
                      }
                  }
              }, () => DocumentNumber != 0);
            Inquiry.ObservesProperty(() => DocumentNumber);
        }
        public BitDelegateCommand Inquiry { get; set; }

        public int DocumentNumber { get; set; }
    }
}
