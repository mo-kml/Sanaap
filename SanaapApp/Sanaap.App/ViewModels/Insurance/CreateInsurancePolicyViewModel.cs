using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Insurance
{
    public class CreateInsurancePolicyViewModel : BitViewModelBase
    {
        private EditMethod method;
        private readonly IODataClient _oDataClient;
        private readonly HttpClient _httpClient;
        private readonly IInitialDataService _initialDataService;
        private readonly IUserDialogs _userDialogs;
        public CreateInsurancePolicyViewModel(IInsurerService insurerService, IUserDialogs userDialogs, HttpClient httpClient, IInitialDataService initialDataService)
        {
            //_oDataClient = oDataClient;
            //_httpClient = httpClient;
            _userDialogs = userDialogs;

            _initialDataService = initialDataService;

            Insurers = insurerService.GetAllInsurers();

            foreach (InsuranceType item in (InsuranceType[])Enum.GetValues(typeof(InsuranceType)))
            {
                InsuranceTypes.Add(new InsuranceTypeItemSource
                {
                    InsuranceType = item,
                    InsuranceTypeName = EnumHelper<InsuranceType>.GetDisplayValue(item)
                });
            }

            Submit = new BitDelegateCommand(async () =>
              {


                  policyCancellationTokenSource?.Cancel();
                  policyCancellationTokenSource = new CancellationTokenSource();

                  using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: policyCancellationTokenSource.Cancel))
                  {
                      if (method == EditMethod.Create)
                      {
                          //await oDataClient.For<InsurancePolicyDto>("InsurancePolicies")
                          //.Set(Policy)
                          //.InsertEntryAsync();
                      }
                      else
                      {
                          //await oDataClient.For<InsurancePolicyDto>("InsurancePolicies")
                          //.Set(Policy)
                          //.UpdateEntryAsync();
                      }
                  }
              });
        }
        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            parameters.TryGetValue("Method", out method);

            if (method == EditMethod.Create)
            {
                SubmitButtonText = "اضافه کردن";
            }
            else
            {
                SubmitButtonText = "ویرایش";
            }

            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

                Colors = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetColors());
            }
        }
        public InsurancePolicyDto Policy { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand SearchInCars { get; set; }

        public string CarSearchText { get; set; }

        public string SubmitButtonText { get; set; }

        public ObservableCollection<ExternalEntityDto> Colors { get; set; }

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public InsurersItemSource[] Insurers { get; set; }

        public List<InsuranceTypeItemSource> InsuranceTypes { get; set; } = new List<InsuranceTypeItemSource>();

        public InsuranceTypeItemSource SelectedInsuranceType { get; set; }

        public CancellationTokenSource policyCancellationTokenSource { get; set; }
    }
    public class InsuranceTypeItemSource
    {
        public InsuranceType InsuranceType { get; set; }

        public string InsuranceTypeName { get; set; }
    }
}
