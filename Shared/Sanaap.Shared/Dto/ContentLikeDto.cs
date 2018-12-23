using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class ContentLikeDto : IDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { set; get; }

        public Guid CustomerId { get; set; }

        public Guid ContentId { get; set; }

        public bool IsLiked { get; set; }
    }
}
