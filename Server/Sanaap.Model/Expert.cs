using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Expert : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string MobileNumber { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        public virtual Guid CityId { get; set; }
    }
}
