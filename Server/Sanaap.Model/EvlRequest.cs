using Sanaap.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class EvlRequest : BaseEntity
    {
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [Required]
        public virtual Guid CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerMobileNumber { get; set; }

        public string VehicleNumber { get; set; }

        [ForeignKey(nameof(VehicleKindId))]
        public VehicleKind VehicleKind { get; set; }

        public int VehicleKindId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public InsuranceType InsuranceTypeEnum { get; set; }

        public DateTimeOffset AccidentDate { get; set; }

        public string InsuranceNumber { get; set; }

        public EvlRequestStatus Status { get; set; }

        public List<EvlRequestFile> Files { get; set; }

        public virtual EvlRequestExpert EvlRequestExpert { get; set; } = new EvlRequestExpert { };
    }
}
