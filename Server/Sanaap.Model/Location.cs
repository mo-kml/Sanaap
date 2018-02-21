using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    [ComplexType]
    public class Location
    {
        public virtual long Lat { get; set; }

        public virtual long Lon { get; set; }
    }
}
