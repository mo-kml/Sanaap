using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Operator : BaseEntity
    {
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string LastName { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(50)]
        public virtual string UserName { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual IList<SosRequestPhone> SosRequestPhones { set; get; }
    }
}
