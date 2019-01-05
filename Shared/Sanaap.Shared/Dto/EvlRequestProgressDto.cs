using Sanaap.Enums;
using System;

namespace Sanaap.Dto
{
    public class EvlRequestProgressDto : ISanaapDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { set; get; }

        public DateTimeOffset ModifiedOn { set; get; }

        public Guid EvlRequestId { get; set; }

        public EvlRequestStatus EvlRequestStatus { get; set; }
    }
}
