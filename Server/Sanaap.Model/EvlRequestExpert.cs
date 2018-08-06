using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    [ComplexType]
    public class EvlRequestExpert
    {
        public string Token { get; set; }
        public int FileID { get; set; }

        //public EvlRequestExpertExpert Expert { get; set; }
        public virtual EvlRequestExpertExpert Expert { get; set; } = new EvlRequestExpertExpert { };

        public string ExpertDistance { get; set; }
    }

    public class EvlRequestExpertExpert
    {
        public int ID { get; set; }
        public int ExpertID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Photo { get; set; }
        public string MapLat { get; set; }
        public string MapLng { get; set; }
        public string TodayRequest { get; set; }
        public string TodayCancelation { get; set; }
        public string TotalRequest { get; set; }
        public string TotalCancelation { get; set; }
        public string Score { get; set; }
        public string LastRequestTime { get; set; }
        public string ConnectionId { get; set; }
        public bool IsBusy { get; set; }
    }
}
