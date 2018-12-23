using Bit.Model.Contracts;
using Sanaap.Enums;
using System;

namespace Sanaap.Dto
{
    public class CommentDto : IDto
    {
        public CommentType CommentType { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string MobileNumber { get; set; }

        public string Description { get; set; }

        public StatusType StatusType { get; set; }

        public string Answer { get; set; }

        public DateTimeOffset AnswerTime { get; set; }

        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { set; get; }

        public DateTimeOffset ModifiedOn { set; get; }
    }
}
