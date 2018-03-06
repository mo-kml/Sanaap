using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public class CustomerMobileDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual long Mobile { get; set; }

        public virtual string MobileStr { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual CustomerDto Customer { get; set; }
        public virtual Guid CustomerId { get; set; }
    }
}