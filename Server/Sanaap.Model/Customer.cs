using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Customer : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string LastName { get; set; }

        [Range(10000000, 9999999999)]
        [Index(IsUnique = true)]
        [Required]
        public virtual long NationalCode { get; set; }

        [Required]
        [Range(1000000000, 9999999999)]
        [Index(IsUnique = true)]
        public virtual long Mobile { get; set; }

        public virtual int OTP { get; set; }

        public virtual bool IsActive { get; set; }
    }
}