using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class User : BaseEntity
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}
