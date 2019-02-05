using PropertyChanged;
using Sanaap.Dto;
using Sanaap.Enums;
using System;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class PolicyItemSource : InsurancePolicyDto
    {
        public string ColorName { get; set; }

        public string CarName { get; set; }

        public string InsuranceTypeName => EnumHelper<InsuranceType>.GetDisplayValue(InsuranceType);

        public LicensePlateItemSource LicensePlateItemSource { get; set; }
    }
}
