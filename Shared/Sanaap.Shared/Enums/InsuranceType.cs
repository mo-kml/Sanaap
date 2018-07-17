using Sanaap.Constants;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public enum InsuranceType
    {
        [Display(Name = EnumDisplayNames.Sales)]
        Sales,

        [Display(Name = EnumDisplayNames.Badane)]
        Badane
    }
}
