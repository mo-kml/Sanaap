using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class InsuranceType : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual IList<EvlRequest> EvlRequests { set; get; }
    }
}