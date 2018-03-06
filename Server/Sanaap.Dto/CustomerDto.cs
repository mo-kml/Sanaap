using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Dto
{
    public class CustomerDto : IDto
    {
        public virtual Guid Id { get; set; }

        [Display(Name = "نام")]
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string FullName { get; set; }

        public virtual long NationalCode { get; set; }

        public virtual int OTP { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual IList<CustomerMobileDto> CustomerMobiles { set; get; }
    }
}