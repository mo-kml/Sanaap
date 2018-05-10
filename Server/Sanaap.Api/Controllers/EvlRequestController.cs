using Bit.Core.Contracts;
using Bit.Data.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestController : DtoController<EvlRequestDto>
    {
        public class SubmitEvlRequestArgs
        {
            public EvlRequestDto evlReq { get; set; }
        }

        public IUserInformationProvider UserInformationProvider { get; set; }

        public IDtoEntityMapper<EvlRequestDto, EvlRequest> Mapper { get; set; }

        public IRepository<EvlRequest> Repository { get; set; }

        [Action]
        public virtual async Task SubmitEvlRequest(SubmitEvlRequestArgs args, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            EvlRequest req = new EvlRequest
            {
                CustomerId = customerId,
                InsuranceTypeId = args.evlReq.InsuranceTypeId,
                Latitude = args.evlReq.Latitude,
                Longitude = args.evlReq.Longitude
            };

            await Repository.AddAsync(req, cancellationToken);
        }
    }
}