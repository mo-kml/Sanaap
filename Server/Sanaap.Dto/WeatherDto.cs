using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sanaap.Dto
{
    public class WeatherDto : IDto
    {        
        [Key]
        public virtual int Temprature { get; set; }
    }
}
