using Sanaap.Enums;

namespace Sanaap.Model
{
    public class InsurancePolicy : BaseEntity
    {
        public int CarId { get; set; }

        public int ColorId { get; set; }

        public string ChasisNo { get; set; }

        public string VIN { get; set; }

        public string InsurerNo { get; set; }

        public string PlateNumber { get; set; }

        public InsuranceType InsuranceType { get; set; }
    }
}
