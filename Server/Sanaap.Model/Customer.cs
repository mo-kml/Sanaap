using System.Collections.Generic;

namespace Sanaap.Model
{
    public class Customer : BaseEntity
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual long NationalCode { get; set; }

        public virtual IList<CustomerMobile> CustomerMobiles { set; get; }
    }
}