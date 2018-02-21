using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Sanaap.Dto;
using Sanaap.Model;
using Sanaap.Service.Contracts;
using Sannap.Data.Contracts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class ExpertsController : SanaapDtoSetController<ExpertDto, Expert>
    {
        public virtual IExpertsRepository ExpertsRepository { get; set; }

        public virtual ISmsService SmsService { get; set; }

        [Function]
        public virtual async Task<IQueryable<ExpertDto>> GetManExperts(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await ExpertsRepository.GetManExprts(cancellationToken));
        }

        public class DeactivateExpertByIdArgs
        {
            public Guid expertId { get; set; }
        }

        [Action]
        public virtual async Task DeactivateExpertById(DeactivateExpertByIdArgs args, CancellationToken cancellation)
        {
            Expert expertToDeactivate = await ExpertsRepository.GetByIdAsync(cancellation, args.expertId);

            if (expertToDeactivate == null)
                throw new ResourceNotFoundException($"Expert with id {args.expertId} not found");

            if (expertToDeactivate.IsActive == false)
                throw new DomainLogicException($"Expert with id {args.expertId} is already deactivated");

            expertToDeactivate.IsActive = false;

            await ExpertsRepository.UpdateAsync(expertToDeactivate, cancellation);

            await SmsService.SendSms("/-:", expertToDeactivate.MobileNumber);
        }

        [Function]
        public virtual async Task<SingleResult<ExpertDto>> GetNearestExpert(CancellationToken cancellationToken)
        {
            return SingleResult.Create(DtoEntityMapper.FromEntityQueryToDtoQuery(await ExpertsRepository.GetNearestExpert(cancellationToken)));
        }
    }
}
