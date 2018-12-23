using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class ContentListDto : IDto
    {
        public byte[] Image { get; set; }

        public string Title { get; set; }

        public int LikesCount { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }
    }
}
