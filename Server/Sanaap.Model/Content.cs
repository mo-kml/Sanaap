using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Content : BaseEntity
    {
        [ForeignKey(nameof(CategoryId))]
        public ContentCategory ContentCategory { get; set; }
        public Guid CategoryId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public byte[] Image { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
