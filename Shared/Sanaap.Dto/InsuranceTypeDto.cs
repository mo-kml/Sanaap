using Bit.Model.Contracts;
using System.Collections.Generic;

namespace Sanaap.Dto
{
    public partial class InsuranceTypeDto : IDto
    {
        public virtual string Name { get; set; }

        public virtual IList<EvlRequestDto> EvlRequests { set; get; }
    }
}