using Bit.Model.Contracts;
using System;
using Sanaap.Dto;

namespace Sanaap.Dto
{
    public partial class SosRequestDto : IDto
    {
        public virtual Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public EnumSosRequestStatus EnumSosRequestStatus { get; set; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }

    public enum EnumSosRequestStatus
    {
        SabteAvalie = 1,
        MoshahedeOperator = 2
    }
}