using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public partial class SosRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public virtual Guid SosRequestStatusId { get; set; }
        public virtual string SosRequestStatusName { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}