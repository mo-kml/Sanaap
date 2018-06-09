using Sanaap.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class SosRequest : BaseEntity
    {
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        [Required]
        public virtual Guid CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public EnumSosRequestStatus SosRequestStatus { get; set; }
    }
}
