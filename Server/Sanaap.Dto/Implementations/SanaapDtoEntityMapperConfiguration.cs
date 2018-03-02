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

            //mapperConfigExpression.CreateMap<Expert, ExpertDto>()
            //    .ForMember(c => c.FullName, cnfg => cnfg.MapFrom(c => c.FirstName + " " + c.LastName));
        }
    }
}
