using Bit.Model.Contracts;
using System;

namespace Sanaap.App.Dto
{
    public class RequestFilesDto : IDto
    {
        public Guid Id { get; set; }

        public Guid EvlExpertRequestId { get; set; }

        public Guid FileTypeId { get; set; }

        public string File { get; set; }
    }
}
