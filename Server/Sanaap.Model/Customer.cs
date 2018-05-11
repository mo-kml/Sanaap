using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Customer : BaseEntity
    {
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string LastName { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public virtual string NationalCode { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(11, MinimumLength = 11)]
        public virtual string Mobile { get; set; }

        public virtual IList<EvlRequest> EvlRequests { set; get; }
    }
}