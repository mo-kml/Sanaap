using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Dto
{
    public class EvlRequestProgressDto : IDto
    {
        [Key]
        public int Stid { set; get; }
        public DateTime STime { set; get; }
        public string Desc { set; get; }
    }
}
