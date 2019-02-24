using Sanaap.Enums;
using System;

namespace Sanaap.Dto
{
    public class InsurancePolicyDto : ISanaapDto
    {
        public Guid Id { get; set; }

        public int CarId { get; set; }

        public int ColorId { get; set; }

        public string ChasisNo { get; set; }

        public string VIN { get; set; }

        public string InsurerNo { get; set; }

        public int InsurerId { get; set; }

        public string PlateNumber { get; set; }

        public DateTimeOffset ExpirationDate { get; set; }

        public InsuranceType InsuranceType { get; set; }

        public Guid CustomerId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}
