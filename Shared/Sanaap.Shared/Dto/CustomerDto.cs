﻿using Bit.Model.Contracts;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class CustomerDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string NationalCode { get; set; }

        public virtual string Mobile { get; set; }

        public virtual List<SosRequestDto> SosRequests { set; get; }
    }
}
