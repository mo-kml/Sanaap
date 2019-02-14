using Bit.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Dto
{
    public class EvlRequestExpertDto : IDto
    {
        [Key]
        public string Token { get; set; }
        public int FileID { get; set; }
        public EvlRequestExpertExpertDto Expert { get; set; }
        public string ExpertDistance { get; set; }
    }

    [ComplexType]
    public class EvlRequestExpertExpertDto
    {
        public int ID { get; set; }
        public int ExpertID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Photo { get; set; }
        public double MapLat { get; set; }
        public double MapLng { get; set; }
        public string CarName { get; set; }
        public string CarPlate { get; set; }
        public int TodayRequest { get; set; }
        public int TodayCancelation { get; set; }
        public int TotalRequest { get; set; }
        public int TotalCancelation { get; set; }
        public int Score { get; set; }
        public DateTime LastRequestTime { get; set; }
        public string ConnectionId { get; set; }
        public bool IsBusy { get; set; }
    }

    public class ExpertResultDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Photo { get; set; }
    }
}
