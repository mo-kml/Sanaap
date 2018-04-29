using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class EvlRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual Guid CustomerId { get; set; }

        public virtual Guid InsuranceTypeId { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}