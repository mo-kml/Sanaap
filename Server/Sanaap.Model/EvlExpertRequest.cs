using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Sanaap.Enums.Enums;

namespace Sanaap.Model
{
    public class EvlExpertRequest : BaseEntity
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

        public InsuranceTypeEnum InsuranceTypeEnum { get; set; }

        public DateTimeOffset AccidentDate { get; set; }

        public string InsuranceNumber { get; set; }

        public EnumRequestStatus SosRequestStatus { get; set; }
    }
}
