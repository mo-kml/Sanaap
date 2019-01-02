using Sanaap.Enums;
using System;

namespace Sanaap.Dto
{
    public partial class SosRequestDto : ISanaapDto
    {
        public virtual Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public EvlRequestStatus SosRequestStatus { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
