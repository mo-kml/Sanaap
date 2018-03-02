using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class CustomerMobileDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual long Mobile { get; set; }

        public virtual string MobileStr { get; set; }
    }
}