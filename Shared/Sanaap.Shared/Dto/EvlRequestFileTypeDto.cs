using Bit.Model.Contracts;
using System;

namespace Sanaap.App.Dto
{
    public class EvlRequestFileTypeDto : IDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
