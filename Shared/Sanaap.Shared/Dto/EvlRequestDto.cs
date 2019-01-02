using Sanaap.Dto;
using Sanaap.Enums;
using System;

namespace Sanaap.App.Dto
{
    public partial class EvlRequestDto : ISanaapDto
    {
        public Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerMobileNumber { get; set; }

        public string VehicleNumber { get; set; }

        public int VehicleKindId { get; set; }

        public int CompanyId { get; set; }

        public InsuranceType InsuranceTypeEnum { get; set; }

        public DateTimeOffset AccidentDate { get; set; }

        public string InsuranceNumber { get; set; }

        public EvlRequestStatus RequestStatus { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}
