using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class CustomerMobile : BaseEntity
    {
        public virtual long Mobile { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        public virtual Guid CustomerId { get; set; }
    }
}