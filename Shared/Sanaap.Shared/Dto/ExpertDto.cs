namespace Sanaap.Dto
{
    public class ExpertDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public string FullName { get => FirstName + " " + LastName; }

        public string Image { get; set; }

        public double Latitude { get; set; }

        public double Lontitude { get; set; }
    }
}
