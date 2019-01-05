using Sanaap.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class EvlRequestProgress : BaseEntity
    {
        [ForeignKey(nameof(EvlRequestId))]
        public EvlRequest EvlRequest { get; set; }
        public Guid EvlRequestId { get; set; }

        public EvlRequestStatus EvlRequestStatus { get; set; }
    }
}
