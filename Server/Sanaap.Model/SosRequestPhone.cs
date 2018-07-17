using Sanaap.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class SosRequestPhone : BaseEntity
    {
        [ForeignKey(nameof(OperatorId))]
        public virtual Operator Operator { get; set; }

        [Required]
        public virtual Guid OperatorId { get; set; }

        public string Description { get; set; }

        public EvlRequestStatus SosRequestStatus { get; set; }
    }
}
