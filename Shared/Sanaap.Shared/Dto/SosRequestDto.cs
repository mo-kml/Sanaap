using Bit.Model.Contracts;
using System;
using static Sanaap.Enums.Enums;

namespace Sanaap.Dto
{
    public partial class SosRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public EnumRequestStatus SosRequestStatus { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}
