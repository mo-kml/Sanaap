using Bit.Model.Contracts;
using Bit.Owin.Contracts;
using Sanaap.App.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    [RoutePrefix("EvlExpertRequests")]
    public class EvlExpertRequestsController : ApiController
    {
        public virtual IUserSettingProvider UserSettingProvider { get; set; }

        public virtual ISanaapRepository<EvlExpertRequest> EvlExpertRequestRepository { get; set; }

        public virtual IDtoEntityMapper<EvlExpertRequestDto, EvlExpertRequest> DtoEntityMapper { get; set; }

        [HttpPost, Route("SaveEvlExpert")]
        public virtual async Task<EvlExpertRequestDto> SaveEvlExpert(EvlExpertRequestDto evlExpertRequest, CancellationToken cancellationToken)
        {
            evlExpertRequest.CustomerId = Guid.Parse((await UserSettingProvider.GetCurrentUserSettingAsync(cancellationToken)).UserId);

            return DtoEntityMapper.FromEntityToDto(await EvlExpertRequestRepository.AddAsync(DtoEntityMapper.FromDtoToEntity(evlExpertRequest), cancellationToken));
        }
    }
}
