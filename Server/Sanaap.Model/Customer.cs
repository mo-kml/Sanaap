using Bit.Model.Contracts;
using Sanaap.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Customer : BaseEntity //, IEntity//, IChangeTrackEnableEntity
    {

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual long NationalCode { get; set; }

        public virtual long Mobile { get; set; }


        //public virtual DateTimeOffset InsertDate { get; set; }
        //public virtual DateTimeOffset EditDate { get; set; }
    }
}