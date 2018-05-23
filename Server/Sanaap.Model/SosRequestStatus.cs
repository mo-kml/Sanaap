using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class SosRequestStatus : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        [Required]
        [Index(IsUnique = true)]
        public virtual string Name { get; set; }

        public virtual int Code { get; set; }

        public virtual IList<SosRequest> SosRequests { set; get; }
    }
}