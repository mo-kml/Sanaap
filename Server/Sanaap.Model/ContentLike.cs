using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class ContentLike : BaseEntity
    {
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }

        [ForeignKey(nameof(ContentId))]
        public Content Content { get; set; }
        public Guid ContentId { get; set; }

        public bool IsLiked { get; set; }
    }
}
