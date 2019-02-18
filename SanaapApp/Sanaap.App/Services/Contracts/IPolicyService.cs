using Sanaap.App.ItemSources;
using Sanaap.Dto;
using Sanaap.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IPolicyService : IService<InsurancePolicyDto>
    {
        Task<List<PolicyItemSource>> LoadAllInsurances();

        Task<List<PolicyItemSource>> LoadAllInsurancesByType(InsuranceType insuranceType);
    }
}
