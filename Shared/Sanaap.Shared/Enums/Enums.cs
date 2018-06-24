using Sanaap.Constants;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public class Enums
    {
        public enum InsuranceTypeEnum
        {
            [Display(Name = DisplayNames.Sales)]
            Sales,

            [Display(Name = DisplayNames.Badane)]
            Badane
        }

        public enum EnumSosRequestStatus
        {
            [Display(Name = DisplayNames.SabteAvalie)]
            SabteAvalie = 1,

            [Display(Name = DisplayNames.TaeenKarshenas)]
            TaeenKarshenas = 2
        }
    }
}
