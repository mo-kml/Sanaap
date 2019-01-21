using Bit.Model;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class CustomerDto : Bindable, ISanaapDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string VerifyCode { get; set; }

        public virtual string CountFailedVerify { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual List<SosRequestDto> SosRequests { set; get; }

        public virtual DateTimeOffset ModifiedOn { set; get; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
