using Bit.Model.Contracts;
using Sanaap.Model;
using System;

namespace Sanaap.Dto
{
    public class CityDto : IDto, ISyncableDto
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual long Version { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual Location Location { get; set; }
    }
}
