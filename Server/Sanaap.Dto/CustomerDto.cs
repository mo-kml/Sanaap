using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public class CustomerDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string FullName { get; set; }

        public virtual long NationalCode { get; set; }

        public virtual IList<CustomerMobileDto> CustomerMobiles { set; get; }
    }
}