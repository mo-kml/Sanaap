namespace Sanaap.Dto
{
    public class FindExpertRequestDto
    {
        public string UserID { get; set; }
        public string RequestID { get; set; }
        public int Type { get; set; }
        public string AccidentDate { get; set; }
        public string Address { get; set; }
        public string MapLat { get; set; }
        public string MapLng { get; set; }
        public int LostInsuranceID { get; set; }
        public string LostInsuranceNO { get; set; }
        public string LostCarType { get; set; }
        public int LostCarID { get; set; }
        public string LostName { get; set; }
        public string LostFamily { get; set; }
        public string LostMobile { get; set; }
    }
}
