using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Model
{
    public class Customer : BaseEntity
    {
        [Display(Name = "نام")]
        [Required]
        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required]
        [MaxLength(50)]
        public virtual string LastName { get; set; }

        [Display(Name = "کد ملی")]
        [Range(10000000, 9999999999)]
        //[Index(IsUnique = true)]
        public virtual long NationalCode { get; set; }

        public virtual int OTP { get; set; }

        [Display(Name = "فعال است؟")]
        public virtual bool IsActive { get; set; }


        public virtual IList<CustomerMobile> CustomerMobiles { set; get; }
    }
}