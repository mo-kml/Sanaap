using AutoMapper;
using Bit.Model.Contracts;
using Sanaap.Model;

namespace Sanaap.Dto.Implementations
{
    public class SanaapDtoEntityMapperConfiguration : IDtoEntityMapperConfiguration
    {
        public virtual void Configure(IMapperConfigurationExpression mapperConfigExpression)
        {
            mapperConfigExpression.ValidateInlineMaps = false;

            mapperConfigExpression.CreateMap<Customer, CustomerDto>()
                .ForMember(src => src.FullName, cnfg => cnfg.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}