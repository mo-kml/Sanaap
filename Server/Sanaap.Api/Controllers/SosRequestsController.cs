﻿using Bit.Core.Contracts;
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
    public class SosRequestsController : DtoController<SosRequestDto>
    {
        public class SubmitSosRequestArgs
        {
            public SosRequestDto sosReq { get; set; }
        }

        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual IDtoEntityMapper<SosRequestDto, SosRequest> Mapper { get; set; }

        public virtual IRepository<SosRequest> Repository { get; set; }

        [Action]
        public virtual async Task SubmitSosRequest(SubmitSosRequestArgs args, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            SosRequest req = new SosRequest
            {
                CustomerId = customerId,
                SosRequestStatus = EvlRequestStatus.SabteAvalie,
                Latitude = args.sosReq.Latitude,
                Longitude = args.sosReq.Longitude,
                Description = args.sosReq.Description
            };

            await Repository.AddAsync(req, cancellationToken);
        }

        [Function]
        public virtual async Task<IQueryable<SosRequestDto>> GetMySosRequests(CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return Mapper.FromEntityQueryToDtoQuery((await Repository
                .GetAllAsync(cancellationToken))
                .Where(sosR => sosR.CustomerId == customerId));
        }
    }
}
