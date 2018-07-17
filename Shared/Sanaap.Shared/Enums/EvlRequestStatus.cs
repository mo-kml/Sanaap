using Sanaap.Constants;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public enum EvlRequestStatus
    {
        [Display(Name = EnumDisplayNames.SabteAvalie)]
        SabteAvalie = 1,

        [Display(Name = EnumDisplayNames.TaeenKarshenas)]
        TaeenKarshenas = 2
    }
}
