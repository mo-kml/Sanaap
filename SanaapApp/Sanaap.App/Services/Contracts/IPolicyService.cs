using Sanaap.App.ViewModels.Insurance;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IPolicyService : IService<InsurancePolicyDto>
    {
        Task<List<PolicyItemSource>> LoadAllInsurances();
    }
}
