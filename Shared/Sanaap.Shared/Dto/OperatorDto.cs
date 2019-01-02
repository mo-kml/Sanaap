using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class OperatorDto : ISanaapDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual List<SosRequestPhoneDto> SosRequestPhoneDtos { set; get; }

        public virtual DateTimeOffset ModifiedOn { set; get; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
