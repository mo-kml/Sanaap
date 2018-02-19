using AutoMapper;
using Bit.Model.Contracts;

namespace Sanaap.Dto.Implementations
{
    public class SanaapDtoEntityMapperConfiguration : IDtoEntityMapperConfiguration
    {
        public virtual void Configure(IMapperConfigurationExpression mapperConfigExpression)
        {
            mapperConfigExpression.ValidateInlineMaps = false;
        }
    }
}
