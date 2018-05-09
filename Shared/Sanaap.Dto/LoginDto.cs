using Bit.Model.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    //[ComplexType]
    public partial class LoginDto : IDto
    {
        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual IList<EvlRequestDto> EvlRequests { set; get; }
    }
}