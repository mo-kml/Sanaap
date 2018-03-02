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
                .ForMember(src => src.FullName, cnfg => cnfg.MapFrom(src => src.FirstName + " " + src.LastName))
                .AfterMap((src, dest) => dest.NationalCodeStr = "abcdefg");
        }

        public string GetNationalCodeStr(long NationalCode)
        {
            string str = NationalCode.ToString();
            int zeroCount = 10 - str.Length;
            string zeros = "";
            for (int i = 0; i < zeroCount; i++)
            {
                zeros += "0";
            }
            return zeros + str;
        }
    }
}