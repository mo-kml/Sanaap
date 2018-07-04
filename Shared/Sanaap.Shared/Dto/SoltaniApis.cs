namespace Sanaap.Dto
{
    public class SoltaniLoginParams
    {
        public string Username { get; set; } = "sanap";
        public string Password { get; set; } = "10431044";
    }

    public class SoltaniLogin
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }

    public class SoltaniFindNearExpertParams
    {
        public string UserID { get; set; }
        public string RequestID { get; set; }
        public string Type { get; set; }
        public string MapLat { get; set; }
        public string MapLng { get; set; }
        public string LostName { get; set; }
        public string LostFamily { get; set; }
        public string LostMobile { get; set; }
        public string LostInsuranceID { get; set; }
        public string LostInsuranceNO { get; set; }
        public string LostCarID { get; set; }
        public string LostCarType { get; set; }
    }

    public class SoltaniFindNearExpert
    {
        public string Token { get; set; }
        public string FileID { get; set; }
        public string ExpName { get; set; }
        public string ExpMobile { get; set; }
        public string ExpMapLat { get; set; }
        public string ExpMapLng { get; set; }
        public string ExpPhoto { get; set; }
    }
}
