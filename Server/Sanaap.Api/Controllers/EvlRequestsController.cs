using Bit.Core.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Newtonsoft.Json;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestsController : SanaapDtoSetController<EvlRequestDto, EvlRequest>
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual ISanaapRepository<EvlRequest> EvlRequestsRepository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestDto, EvlRequest> Mapper { get; set; }


        public class UpdateRequestStatusArgs
        {
            public Guid evlRequestId { get; set; }

            public EvlRequestStatus status { get; set; }
        }

        [Action]
        public virtual async Task UpdateRequestStatus(UpdateRequestStatusArgs args, CancellationToken cancellationToken)
        {
            EvlRequest evlReq = await EvlRequestsRepository.GetByIdAsync(cancellationToken, args.evlRequestId);
            evlReq.Status = args.status;
            await EvlRequestsRepository.UpdateAsync(evlReq, cancellationToken);
        }

        [Function]
        public virtual async Task<IQueryable<EvlRequestDto>> GetCustomerEvlRequests(CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return Mapper.FromEntityQueryToDtoQuery((await EvlRequestsRepository.GetAllAsync(cancellationToken)).Where(r => r.CustomerId == customerId));
        }

        [Function]
        public virtual async Task<List<EvlRequest>> GetExpertEvlRequests(int expertId, CancellationToken cancellationToken)
        {
            return (await EvlRequestsRepository.GetAllAsync(cancellationToken)).Where(r => r.EvlRequestExpert.Expert.ExpertID == expertId).ToList();
        }

        //[Function]
        //public virtual async Task<List<EvlRequest>> GetCustomerEvlRequests(Guid customerId, CancellationToken cancellationToken)
        //{
        //    return (await EvlRequestsRepository.GetAllAsync(cancellationToken)).Where(r => r.EvlRequestExpert. == customerId).ToList();
        //}

    }

    [RoutePrefix("evl-requests")]
    public class EvlRequestsApiController : ApiController
    {
        public virtual ISanaapRepository<EvlRequestFile> EvlRequestFilesRepository { get; set; }

        public virtual ISanaapRepository<EvlRequest> EvlRequestsRepository { get; set; }

        public virtual ISanaapRepository<EvlRequestProgress> EvlRequestProgressesRepository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestDto, EvlRequest> Mapper { get; set; }

        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        [HttpPost, Route("submit-evl-request")]
        public virtual async Task<EvlRequestDto> SubmitEvlRequest(CancellationToken cancellationToken)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider, cancellationToken);

            bool isFirstPart = true;
            EvlRequestDto evlRequestDto = null;

            foreach (HttpContent requestPart in provider.Contents)
            {
                if (isFirstPart == true)
                {
                    evlRequestDto = JsonConvert.DeserializeObject<EvlRequestDto>(await requestPart.ReadAsStringAsync());

                    evlRequestDto.Status = EvlRequestStatus.SabteAvalie;

                    EvlRequest evlRequest = Mapper.FromDtoToEntity(evlRequestDto);
                    evlRequest.CustomerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());
                    evlRequest.Code = await EvlRequestsRepository.GetNextSequenceValue();

                    evlRequestDto = Mapper.FromEntityToDto(await EvlRequestsRepository.AddAsync(evlRequest, cancellationToken));

                    await EvlRequestProgressesRepository.AddAsync(new EvlRequestProgress { EvlRequestId = evlRequestDto.Id, EvlRequestStatus = EvlRequestStatus.SabteAvalie }, cancellationToken);

                    isFirstPart = false;

                    continue;
                }
                else
                {
                    byte[] data = await requestPart.ReadAsByteArrayAsync();

                    int typeId = int.Parse(Path.GetFileName(requestPart.Headers.ContentDisposition.FileName.Trim('\"')));

                    await EvlRequestFilesRepository.AddAsync(new EvlRequestFile
                    {
                        EvlRequestId = evlRequestDto.Id,
                        TypeId = typeId,
                        File = data
                    }, cancellationToken);
                }
            }

            if (evlRequestDto == null)
            {
                throw new BadRequestException($"{nameof(EvlRequestDto)} is null");
            }

            return evlRequestDto;
        }
    }
}
