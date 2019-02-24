using AutoMapper;
using Bit.Core.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Newtonsoft.Json;
using Sanaap.Api.Contracts;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Model;
using Sanaap.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestExpertsController : DtoController<EvlRequestExpertDto>
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual ISanaapRepository<EvlRequest> EvlRequestsRepository { get; set; }

        public virtual ISanaapRepository<EvlRequestFile> EvlRequestFileRepository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestDto, EvlRequest> EvlRequestDtoEntityMapper { get; set; }

        public virtual IDtoEntityMapper<EvlRequestExpertDto, EvlRequestExpert> EvlRequestExpertDtoEntityMapper { get; set; }

        public virtual IMapper Mapper { get; set; }

        public virtual IExternalApiService ExternalApiService { get; set; }

        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }

        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        public class FindEvlReqeustExpertArgs
        {
            public Guid requestId { get; set; }
        }

        [Action]
        public virtual async Task<EvlRequestExpertDto> FindEvlRequestExpert(FindEvlReqeustExpertArgs args, CancellationToken cancellationToken)
        {
            EvlRequest evlRequest = await (await EvlRequestsRepository.GetAllAsync(cancellationToken)).Where(e => e.Id == args.requestId).Include(c => c.Customer).FirstOrDefaultAsync();

            if (evlRequest == null)
            {
                throw new ResourceNotFoundException("evlRequest is null");
            }

            List<EvlRequestFile> requestFiles = await (await EvlRequestFileRepository.GetAllAsync(cancellationToken)).Where(e => e.EvlRequestId == evlRequest.Id).ToListAsync();

            HttpClient httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            SoltaniFindExpertRequest soltaniFindExpertParams = new SoltaniFindExpertRequest();
            soltaniFindExpertParams.UserID = evlRequest.CustomerId;
            soltaniFindExpertParams.RequestID = evlRequest.Id;
            soltaniFindExpertParams.Type = (short)evlRequest.InsuranceType;
            DefaultDateTimeUtils defaultDateTimeUtils = new DefaultDateTimeUtils();
            soltaniFindExpertParams.AccidentDate = defaultDateTimeUtils.ConvertMiladiToShamsi(evlRequest.AccidentDate);
            soltaniFindExpertParams.MapLat = evlRequest.Latitude;
            soltaniFindExpertParams.MapLng = evlRequest.Longitude;
            soltaniFindExpertParams.Address = "آدرس تستی";

            requestFiles.ForEach(r => soltaniFindExpertParams.Photos.Add(new RequestPhoto
            {
                Data = Convert.ToBase64String(r.File),
                Type = r.TypeId
            }));

            if (evlRequest.InsuranceType == Enums.InsuranceType.Badane)
            {
                soltaniFindExpertParams.LostName = evlRequest.OwnerFirstName;
                soltaniFindExpertParams.LostFamily = evlRequest.OwnerLastName;
                soltaniFindExpertParams.LostInsuranceID = evlRequest.InsurerId;
                soltaniFindExpertParams.LostCarID = evlRequest.CarId;
                soltaniFindExpertParams.LostMobile = evlRequest.Customer.Mobile;
            }
            else
            {
                soltaniFindExpertParams.LostName = evlRequest.LostFirstName;
                soltaniFindExpertParams.LostFamily = evlRequest.LostLastName;
                soltaniFindExpertParams.LostMobile = evlRequest.Customer.Mobile;
                soltaniFindExpertParams.LostCarID = evlRequest.LostCarId;

                soltaniFindExpertParams.CrmName = evlRequest.OwnerFirstName;
                soltaniFindExpertParams.CrmFamily = evlRequest.OwnerLastName;
                soltaniFindExpertParams.CrmInsuranceID = evlRequest.InsurerId;
                soltaniFindExpertParams.CrmInsuranceNO = evlRequest.InsurerNo;

                soltaniFindExpertParams.CrmNumberplate = evlRequest.PlateNumber;
                soltaniFindExpertParams.CrmCarID = evlRequest.CarId;
            }
            string json = JsonConvert.SerializeObject(soltaniFindExpertParams);

            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage findExpertRawResponse = await httpClient.PostAsync("FindNearExpert", stringContent);
            try
            {
                findExpertRawResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new DomainLogicException("FindNearExpert call failed", ex);
            }



            EvlRequestExpertDto evlRequestExpert = JsonConvert.DeserializeObject<EvlRequestExpertDto>(await findExpertRawResponse.Content.ReadAsStringAsync());

            evlRequest.Code = evlRequestExpert.FileID;

            await EvlRequestsRepository.UpdateAsync(evlRequest, cancellationToken);

            return evlRequestExpert;
        }
        public class GetPositionArgs
        {
            public string Token { get; set; }
        }

        [Action]
        public virtual async Task<string> GetExpertPosition([FromBody]GetPositionArgs args)
        {
            return JsonConvert.SerializeObject(await ExternalApiService.GetExpertPosition(args));
        }
    }
    public class RequestPhoto
    {
        public int Type { set; get; }
        public string Data { set; get; }

    }
    public class SoltaniFindExpertRequest
    {
        public Guid UserID { set; get; }
        public Guid RequestID { set; get; }
        public int Type { set; get; }
        public string AccidentDate { set; get; }
        public double MapLat { set; get; }
        public double MapLng { set; get; }
        public string Address { set; get; }
        public bool TestMode { set; get; }
        public bool FromPortal { set; get; }
        //Criminal
        public int CrmInsuranceID { set; get; }
        public string CrmInsuranceNO { set; get; }
        public int CrmCarID { set; get; }
        public int CrmCarType { set; get; }
        public int CrmNumberplateType { set; get; }
        public string CrmNumberplate { set; get; }
        public string CrmName { set; get; }
        public string CrmFamily { set; get; }
        public string CrmMobile { set; get; }
        //Lost
        public int LostInsuranceID { set; get; }
        public string LostInsuranceNO { set; get; }
        public int LostCarID { set; get; }
        public int LostCarType { set; get; }
        public int LostNumberplateType { set; get; }
        public string LostNumberplate { set; get; }
        public string LostName { set; get; }
        public string LostFamily { set; get; }
        public string LostMobile { set; get; }

        public List<RequestPhoto> Photos { set; get; }

        public SoltaniFindExpertRequest()
        {
            Photos = new List<RequestPhoto>();
        }
    }
}
