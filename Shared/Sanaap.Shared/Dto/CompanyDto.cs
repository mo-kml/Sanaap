using Bit.Model.Contracts;

namespace Sanaap.App.Dto
{
    public class CompanyDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
