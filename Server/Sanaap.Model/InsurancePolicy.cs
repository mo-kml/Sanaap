using Sanaap.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class InsurancePolicy : BaseEntity
    {
        public int CarId { get; set; }

        public int ColorId { get; set; }

        public string ChasisNo { get; set; }

        public string VIN { get; set; }

        public string InsurerNo { get; set; }

        public int InsurerId { get; set; }

        public string PlateNumber { get; set; }

        public DateTimeOffset ExpirationDate { get; set; }

        public InsuranceType InsuranceType { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
    }
}
