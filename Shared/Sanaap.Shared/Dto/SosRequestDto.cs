using Bit.Model.Contracts;
using Sanaap.Enum;
using System;

namespace Sanaap.Dto
{
    public partial class SosRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public EnumSosRequestStatus SosRequestStatus { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}
