using Bit.Core.Contracts;
using Bit.Data.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class InsurancePoliciesController : SanaapDtoSetController<InsurancePolicyDto, InsurancePolicy>
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual IDtoEntityMapper<InsurancePolicyDto, InsurancePolicy> Mapper { get; set; }

        public virtual IRepository<InsurancePolicy> Repository { get; set; }

        public override Task<InsurancePolicyDto> Create(InsurancePolicyDto dto, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            dto.CustomerId = customerId;

            return base.Create(dto, cancellationToken);
        }

        public override Task<InsurancePolicyDto> Update(Guid key, InsurancePolicyDto dto, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            dto.CustomerId = customerId;

            return base.Update(key, dto, cancellationToken);
        }

        [Function]
        public virtual async Task<IQueryable<InsurancePolicyDto>> LoadInsurances(CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return Mapper.FromEntityQueryToDtoQuery((await Repository
                .GetAllAsync(cancellationToken))
                .Where(insurance => insurance.CustomerId == customerId));
        }

        [Function]
        public virtual async Task<IQueryable<InsurancePolicyDto>> LoadInsurancesByType(InsuranceType insuranceType, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return Mapper.FromEntityQueryToDtoQuery((await Repository
                .GetAllAsync(cancellationToken))
                .Where(insurance => insurance.CustomerId == customerId && insurance.InsuranceType == insuranceType));
        }
    }
}
