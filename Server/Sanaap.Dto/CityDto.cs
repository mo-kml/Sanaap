using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class CityDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
    }
}
