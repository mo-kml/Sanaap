using Sanaap.Enums;
using System;

namespace Sanaap.Dto
{
    public partial class EvlRequestDto : ISanaapDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public virtual Guid CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string AccidentReason { get; set; }

        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public string InsurerNo { get; set; }

        public int InsurerId { get; set; }

        public int CarId { get; set; }

        public string PlateNumber { get; set; }

        public InsuranceType InsuranceType { get; set; }

        public DateTimeOffset AccidentDate { get; set; }

        public EvlRequestStatus Status { get; set; }

        //زیان دیده
        public string LostFirstName { get; set; }

        public string LostLastName { get; set; }

        public int LostCarId { get; set; }

        public string LostPlateNumber { get; set; }
    }
}
