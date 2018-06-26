using Bit.Model.Contracts;
using System;
using static Sanaap.Enums.Enums;

namespace Sanaap.App.Dto
{
    public class EvlExpertRequestDto : IDto
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerMobileNumber { get; set; }

        public string VehicleNumber { get; set; }

        public int VehicleKindId { get; set; }

        public int CompanyId { get; set; }

        public InsuranceTypeEnum InsuranceTypeEnum { get; set; }

        public DateTimeOffset AccidentDate { get; set; }

        public string InsuranceNumber { get; set; }

        public EnumSosRequestStatus SosRequestStatus { get; set; }
    }
}
