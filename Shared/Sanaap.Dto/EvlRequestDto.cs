using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public partial class EvlRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual Guid InsuranceTypeId { get; set; }

        public virtual Guid CarTypeId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }

        public virtual string InsuranceTypeName { get; set; }
    }
}