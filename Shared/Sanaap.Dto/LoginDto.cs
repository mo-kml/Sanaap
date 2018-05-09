using Bit.Model.Contracts;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class LoginDto : IDto
    {
        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual IList<EvlRequestDto> EvlRequests { set; get; }
    }
}