using Bit.Model.Contracts;

namespace Sanaap.Dto
{
    public class InsurerDto : IDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public bool IsContract { get; set; }
    }
}
