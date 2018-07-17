using Sanaap.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public abstract class BaseEntity : IChangeTrackEnableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual DateTimeOffset CreatedOn { set; get; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}
