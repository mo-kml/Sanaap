using Bit.Model.Contracts;
using System;

namespace Sanaap.Dto
{
    public interface ISanaapDto : IDto
    {
        Guid Id { get; set; }

        DateTimeOffset CreatedOn { set; get; }

        DateTimeOffset ModifiedOn { set; get; }
    }
}
