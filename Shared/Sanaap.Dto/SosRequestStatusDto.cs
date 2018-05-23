using Bit.Model.Contracts;
using System;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class SosRequestStatusDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int Code { get; set; }

        public virtual IList<SosRequestStatusDto> SosRequestStatuses { set; get; }
    }
}