using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class RequestFiles : BaseEntity
    {
        [ForeignKey(nameof(EvlExpertRequestId))]
        public EvlExpertRequest EvlExpertRequest { get; set; }
        public Guid EvlExpertRequestId { get; set; }

        [ForeignKey(nameof(FileTypeId))]
        public FileType FileType { get; set; }
        public Guid FileTypeId { get; set; }

        public string File { get; set; }
    }
}
