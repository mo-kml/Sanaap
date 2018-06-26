using Bit.Model.Contracts;
using System;

namespace Sanaap.App.Dto
{
    public class FileTypeDto : IDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
