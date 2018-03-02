﻿using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class CustomerDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string FullName { get; set; }

        public virtual long NationalCode { get; set; }

        public virtual string NationalCodeStr { get; set; }
    }
}