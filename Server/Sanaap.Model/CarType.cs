using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class CarType : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        [Required]
        [Index(IsUnique = true)]
        public virtual string Name { get; set; }

        public virtual IList<EvlRequest> EvlRequests { set; get; }

        public virtual int Code { get; set; }
    }
}