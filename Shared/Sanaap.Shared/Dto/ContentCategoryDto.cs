using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public class ContentCategoryDto : IDto
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { set; get; }

        public DateTimeOffset ModifiedOn { set; get; }
    }
}
