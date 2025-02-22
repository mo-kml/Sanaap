﻿using PropertyChanged;
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

        public string Image { get; set; }

        public bool IsExpired { get; set; }

        public LicensePlateItemSource LicensePlateItemSource { get; set; }
    }
}
