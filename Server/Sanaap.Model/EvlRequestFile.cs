﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class EvlRequestFile : BaseEntity
    {
        [ForeignKey(nameof(EvlRequestId))]
        public EvlRequest EvlRequest { get; set; }
        public Guid EvlRequestId { get; set; }

        public int TypeId { get; set; }

        public byte[] File { get; set; }
    }
}
