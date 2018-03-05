using AutoMapper;
using Bit.Model.Contracts;
using Sanaap.Model;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Sanaap.Dto.Implementations
{
    public class SanaapDtoEntityMapperConfiguration : IDtoEntityMapperConfiguration
    {
        public virtual void Configure(IMapperConfigurationExpression mapperConfigExpression)
        {
            mapperConfigExpression.ValidateInlineMaps = false;

            mapperConfigExpression.CreateMap<Customer, CustomerDto>()
                .ForMember(src => src.FullName, cnfg => cnfg.MapFrom(src => src.FirstName + " " + src.LastName));

            // IsMovedToBit
            mapperConfigExpression.ForAllPropertyMaps(p => (p.DestinationProperty.GetCustomAttribute<ForeignKeyAttribute>() != null || p.DestinationProperty.GetCustomAttribute<InversePropertyAttribute>() != null)
                       && !typeof(IEnumerable).IsAssignableFrom(p.DestinationProperty.ReflectedType)
                       && typeof(IDto).IsAssignableFrom(p.DestinationProperty.ReflectedType),
                (pConfig, member) =>
                {
                    pConfig.Ignored = true;
                });
        }
    }
}