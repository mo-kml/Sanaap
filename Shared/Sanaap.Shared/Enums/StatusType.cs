using System.ComponentModel.DataAnnotations;

namespace Sanaap.Enums
{
    public enum StatusType
    {
        [Display(Name = "پذیرفته")]
        Accept,

        [Display(Name = "شکست خورده")]
        Failed
    }
}
