using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class BaseEntity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public DateTimeOffset AddDate { set; get; }
        public DateTimeOffset? EditDate { set; get; }

        //public EntityState State { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { set; get; }
    }
}
