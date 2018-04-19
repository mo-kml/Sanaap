using AutoMapper;
using Bit.Model.Contracts;
using Sanaap.Dto;
using Sanaap.Model;

namespace Sanaap.Api.Implementations
{
    public class SanaapDtoEntityMapperConfiguration : IDtoEntityMapperConfiguration
    {
        public virtual void Configure(IMapperConfigurationExpression mapperConfigExpression)
        {
            mapperConfigExpression.CreateMap<Customer, CustomerDto>()
                .ForMember(src => src.FullName, cnfg => cnfg.MapFrom(src => src.FirstName + " " + src.LastName));

            mapperConfigExpression.CreateMap<CustomerDto, Customer>()
                .ForMember(c => c.Id, cnfg => cnfg.Ignore())
                .ForMember(c => c.OTP, cnfg => cnfg.Ignore())
                .ForMember(c => c.IsActive, cnfg => cnfg.Ignore());
        }
    }
}