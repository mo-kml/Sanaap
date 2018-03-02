namespace Sanaap.Model
{
    public class CustomerMobile : BaseEntity
    {
        public virtual long Mobile { get; set; }

        public virtual Customer Customer { set; get; }
    }
}