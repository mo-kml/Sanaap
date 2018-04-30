using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    /// <summary>
    /// Evaluation Request
    /// </summary>
    public class EvlRequest : BaseEntity
    {
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [Required]
        public virtual Guid CustomerId { get; set; }

        [ForeignKey(nameof(InsuranceTypeId))]
        public virtual InsuranceType InsuranceType { get; set; }

        public virtual Guid InsuranceTypeId { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}