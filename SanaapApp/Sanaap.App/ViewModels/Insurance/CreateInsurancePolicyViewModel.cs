using Bit.ViewModel;
using Sanaap.App.ItemSources;
using Sanaap.Dto;
using System.Collections.ObjectModel;

namespace Sanaap.App.ViewModels.Insurance
{
    public class CreateInsurancePolicyViewModel : BitViewModelBase
    {
        public InsurancePolicyDto Policy { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public string SubmitButtonText { get; set; }

        public ObservableCollection<ExternalEntityDto> Colors { get; set; }

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public ObservableCollection<InsurersItemSource> Insurers { get; set; }
    }
}
