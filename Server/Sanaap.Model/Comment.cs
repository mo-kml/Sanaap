using Sanaap.Enums;
using System;

namespace Sanaap.Model
{
    //انتقادات و پیشنهادات
    public class Comment : BaseEntity
    {
        public CommentType CommentType { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string MobileNumber { get; set; }

        public string Description { get; set; }

        public StatusType StatusType { get; set; }

        public string Answer { get; set; }

        public DateTimeOffset AnswerTime { get; set; }

    }
}
