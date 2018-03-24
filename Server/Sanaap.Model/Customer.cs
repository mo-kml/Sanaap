using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Index(IsUnique = true)]
        [Required]
        public virtual long NationalCode { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required]
        [Range(1000000000, 9999999999)]
        public virtual long Mobile { get; set; }

        public virtual int OTP { get; set; }

        [Display(Name = "فعال است؟")]
        public virtual bool IsActive { get; set; }
    }
}