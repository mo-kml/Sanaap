using Sanaap.Constants;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public enum InsuranceType
    {
        [Display(Name = EnumDisplayNames.Sales)]
        Sales = 2,

        [Display(Name = EnumDisplayNames.Badane)]
        Badane = 1
    }
}
