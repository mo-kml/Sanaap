using Bit.Core.Contracts;
using Bit.Data.Contracts;
using Bit.Model.Contracts;
using Sanaap.Dto;
using Sanaap.Model;
using System;
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
    }
}
