using Bit.Model.Contracts;
using System;

namespace Sanaap.Model.Contracts
{
    public interface IChangeTrackEnableEntity : IEntity
    {
        DateTimeOffset CreatedOn { set; get; }
        DateTimeOffset ModifiedOn { set; get; }
    }
}
