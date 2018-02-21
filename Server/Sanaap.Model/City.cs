using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class City : IEntity, ISyncableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual long Version { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual Location Location { get; set; }
    }
}
