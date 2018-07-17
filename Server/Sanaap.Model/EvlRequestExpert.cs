using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    [ComplexType]
    public class EvlRequestExpert
    {
        public string Token { get; set; }
        public string FileID { get; set; } // ?
        public string ExpName { get; set; }
        public string ExpMobile { get; set; }
        public string ExpMapLat { get; set; }
        public string ExpMapLng { get; set; }
        public string ExpPhoto { get; set; }
    }
}
