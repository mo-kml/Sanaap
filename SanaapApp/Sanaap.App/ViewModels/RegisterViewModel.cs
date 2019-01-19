using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Threading;
using System.Timers;

namespace Sanaap.App.ViewModels
{
    public class RegisterViewModel : BitViewModelBase
    {
        public int TimerValue { get; set; }

        public bool IsEnterDataVisible { get; set; } = true;

        public bool IsVerifySectionVisible { get; set; }

        public string VerificationCode { get; set; }

        public virtual BitDelegateCommand Login { get; set; }

        public virtual BitDelegateCommand Register { get; set; }

        public BitDelegateCommand SendVerificationCodeAgain { get; set; }

        public BitDelegateCommand VerifyCode { get; set; }

        public virtual CustomerDto Customer { get; set; } = new CustomerDto { };

        private CancellationTokenSource registerCancellationTokenSource;

        public RegisterViewModel(INavService navigationService,
            IODataClient oDataClient,
            ICustomerValidator customerValidator,
            IPageDialogService pageDialogService,
            ISecurityService securityService,
            ISanaapAppTranslateService translateService,
            IUserDialogs userDialogs)
        {
            //Login = new BitDelegateCommand(async () =>
            //{
            //    await navigationService.NavigateAsync("Login");
            //});

            Register = new BitDelegateCommand(async () =>
            {
                registerCancellationTokenSource?.Cancel();
                registerCancellationTokenSource = new CancellationTokenSource();

                using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
                {
                    if (!customerValidator.IsValid(Customer, out string errorMessage))
                    {
                        await pageDialogService.DisplayAlertAsync("", translateService.Translate(errorMessage), "باشه");
                        return;
                    }

                    try
                    {
                        Customer = await oDataClient.For<CustomerDto>("Customers")
                            .Action("RegisterCustomer")
                            .Set(new
                            {
                                customer = Customer
                            })
                            .ExecuteAsSingleAsync(registerCancellationTokenSource.Token);

                        IsEnterDataVisible = false;

                        IsVerifySectionVisible = true;

                        startVerifyTimer();
                    }
                    catch (Exception ex)
                    {
                        if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                        {
                            await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            });

            SendVerificationCodeAgain = new BitDelegateCommand(async () =>
              {
                  registerCancellationTokenSource?.Cancel();
                  registerCancellationTokenSource = new CancellationTokenSource();

                  using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
                  {
                      try
                      {
                          Customer = await oDataClient.For<CustomerDto>("Customers")
                              .Action("SendVerificationCodeAgain")
                              .Set(new
                              {
                                  customer = Customer
                              })
                              .ExecuteAsSingleAsync(registerCancellationTokenSource.Token);

                          startVerifyTimer();
                      }
                      catch (Exception ex)
                      {
                          if (translateService.Translate(ex.GetMessage(), out string translateErrorMessage))
                          {
                              await pageDialogService.DisplayAlertAsync("", translateErrorMessage, "باشه");
                          }
                          else
                          {
                              throw;
                          }
                      }
                  }
              }, () => TimerValue == 0);

            SendVerificationCodeAgain.ObservesProperty(() => TimerValue);

            VerifyCode = new BitDelegateCommand(async () =>
              {
                  if (VerificationCode == Customer.VerifyCode)
                  {
                      IsVerifySectionVisible = false;

                      await securityService.LoginWithCredentials(Customer.NationalCode, Customer.Mobile, "SanaapResOwner", "secret", cancellationToken: registerCancellationTokenSource.Token);

                      await navigationService.NavigateAsync("/Menu/Nav/Main");
                  }
                  else
                  {
                      await pageDialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.InvalidVerifyCode, ConstantStrings.Ok);

                      VerificationCode = string.Empty;
                  }
              });
        }
        public void startVerifyTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            TimerValue = 60;
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (TimerValue > 0)
            {
                TimerValue--;
            }
            else
            {
                ((System.Timers.Timer)sender).Stop();
            }
        }
    }
}
