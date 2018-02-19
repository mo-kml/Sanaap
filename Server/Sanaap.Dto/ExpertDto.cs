using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class ExpertDto : IDto
    {
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
    }
}
