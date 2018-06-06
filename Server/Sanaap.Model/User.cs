using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class User : IEntity
    {
        public virtual Guid Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50, MinimumLength = 3)]
        public virtual string UserName { get; set; }

        [Required]
        public virtual string Password { get; set; }
    }
}
