using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public class ExpertDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual string FullName { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual CityDto City { get; set; }

        public virtual Guid CityId { get; set; }
    }
}
