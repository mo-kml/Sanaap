using Sanaap.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    //انتقادات و پیشنهادات
    public class Comment : BaseEntity
    {
        public CommentType CommentType { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Mobile { get; set; }

        public string Description { get; set; }

        public StatusType StatusType { get; set; }

        public string Answer { get; set; }

        public DateTimeOffset AnswerTime { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }

        public long Code { get; set; }
    }
}
