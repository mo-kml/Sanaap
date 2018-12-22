using Sanaap.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class Customer : BaseEntity
    {
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public virtual string LastName { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public virtual string NationalCode { get; set; }

        public SexType SexType { get; set; }

        public string MobileOS { get; set; }

        public DateTimeOffset LastestLogin { get; set; }

        public string VerifyCode { get; set; }

        public string CountFailedVerify { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(11, MinimumLength = 11)]
        public virtual string Mobile { get; set; }

        public virtual List<SosRequest> SosRequests { set; get; }

        public virtual List<EvlRequest> EvlRequests { get; set; }
    }
}
