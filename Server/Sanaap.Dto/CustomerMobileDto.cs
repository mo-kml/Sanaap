using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public class CustomerMobileDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual long Mobile { get; set; }

        public virtual string MobileStr { get; set; }

        public virtual Guid Customer_Id { get; set; }
    }
}