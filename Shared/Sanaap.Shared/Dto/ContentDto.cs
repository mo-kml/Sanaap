using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class ContentDto : IDto
    {
        public Guid CategoryId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public byte[] Image { get; set; }

        public Guid UserId { get; set; }

        public virtual Guid Id { get; set; }

        public virtual DateTimeOffset CreatedOn { set; get; }

        public virtual DateTimeOffset ModifiedOn { set; get; }
    }
}
