using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public partial class EvlRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual Guid CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual CustomerDto Customer { get; set; }

        public virtual Guid InsuranceTypeId { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}