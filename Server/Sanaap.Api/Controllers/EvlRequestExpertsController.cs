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
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using System;
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
            EvlRequest evlRequest = await EvlRequestsRepository.GetByIdAsync(cancellationToken, args.requestId);
            if (evlRequest == null)
            {
                throw new ResourceNotFoundException("evlRequest is null");
            }
            //if (!string.IsNullOrEmpty(evlRequest.EvlRequestExpert.Token))
            //{
            //    return SingleResult.Create(new EvlRequestExpertDto[] Mapper.Map<EvlRequestExpertDto>(evlRequest.EvlRequestExpert);
            //}


            HttpClient httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");

            Customer customer = await CustomersRepository.GetByIdAsync(cancellationToken, Guid.Parse(UserInformationProvider.GetCurrentUserId()));

            SoltaniFindExpertRequest soltaniFindExpertParams = new SoltaniFindExpertRequest();
            soltaniFindExpertParams.UserID = UserInformationProvider.GetCurrentUserId();
            soltaniFindExpertParams.RequestID = evlRequest.Id.ToString();
            soltaniFindExpertParams.Type = (short)evlRequest.InsuranceType;
            DefaultDateTimeUtils defaultDateTimeUtils = new DefaultDateTimeUtils();
            soltaniFindExpertParams.AccidentDate = defaultDateTimeUtils.ConvertMiladiToShamsi(evlRequest.AccidentDate);
            soltaniFindExpertParams.MapLat = evlRequest.Latitude;
            soltaniFindExpertParams.MapLng = evlRequest.Longitude;
            soltaniFindExpertParams.LostName = evlRequest.LostFirstName ?? "a";
            soltaniFindExpertParams.LostFamily = evlRequest.LostLastName ?? "a";
            soltaniFindExpertParams.LostMobile = "0";
            soltaniFindExpertParams.LostInsuranceID = 1;
            soltaniFindExpertParams.LostCarID = evlRequest.LostCarId == 0 ? 1 : evlRequest.LostCarId; // 12608
            soltaniFindExpertParams.Address = "111";

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(soltaniFindExpertParams), UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage findExpertRawResponse = await httpClient.PostAsync("FindNearExpert", stringContent);
            try
            {
                findExpertRawResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new DomainLogicException("FindNearExpert call failed", ex);
            }

            return JsonConvert.DeserializeObject<EvlRequestExpertDto>(await findExpertRawResponse.Content.ReadAsStringAsync());

            //evlRequest.EvlRequestExpert = Mapper.Map<EvlRequestExpert>(evlRequestExpertDto);
            //await EvlRequestsRepository.UpdateAsync(evlRequest, cancellationToken);
            //EvlRequestExpertExpertDto result = Mapper.Map<EvlRequestExpertExpertDto>(evlRequestExpertDto.Expert);

            //evlRequest.EvlRequestExpert.Token = evlRequestExpertDto.Token;
            //evlRequest.EvlRequestExpert.FileID = evlRequestExpertDto.FileID;
            //evlRequest.EvlRequestExpert.Expert.ID = result.ID;
            //evlRequest.EvlRequestExpert.Expert.ExpertID = result.ExpertID;
            //evlRequest.EvlRequestExpert.Expert.Name = result.Name;
            //evlRequest.EvlRequestExpert.Expert.Mobile = result.Mobile;
            //evlRequest.EvlRequestExpert.Expert.MapLat = result.MapLat;
            //evlRequest.EvlRequestExpert.Expert.MapLng = result.MapLng;
            //evlRequest.EvlRequestExpert.Expert.Photo = result.Photo;

            //EvlRequestsRepository.Update(evlRequest);

            //return evlRequestExpertDto;
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




    public class SoltaniFindExpertRequest
    {
        public string UserID { get; set; }
        public string RequestID { get; set; }
        public int Type { get; set; }
        public string AccidentDate { get; set; }
        public string Address { get; set; }
        public double MapLat { get; set; }
        public double MapLng { get; set; }
        public int LostInsuranceID { get; set; }
        public int LostCarID { get; set; }
        public string LostName { get; set; }
        public string LostFamily { get; set; }
        public string LostMobile { get; set; }
    }

    [RoutePrefix("EvlRequestExperts")]
    public class EvlRequestExpertsApiController : ApiController
    {
        public virtual ISanaapRepository<EvlRequest> EvlRequestsRepository { get; set; }
        public virtual IHttpClientFactory HttpClientFactory { get; set; }
        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }
        public virtual IUserInformationProvider UserInformationProvider { get; set; }


        [HttpPost, Route("FindNearExpert")]
        public virtual async Task<EvlRequestExpertExpertDto> FindNearExpert(Guid evlRequestId, CancellationToken cancellationToken)
        {
            EvlRequest evlRequest = await EvlRequestsRepository.GetByIdAsync(cancellationToken, evlRequestId);
            if (evlRequest == null)
            {
                throw new ResourceNotFoundException("evlRequest is null");
            }

            if (!string.IsNullOrEmpty(evlRequest.EvlRequestExpert.Token))
            {
                return Mapper.Map<EvlRequestExpertExpertDto>(evlRequest.EvlRequestExpert);
            }


            HttpClient httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            var soltaniLoginParams = new { Username = "sanap", Password = "10431044" };
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(soltaniLoginParams), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage soltaniLoginRawResponse;
            try
            {
                soltaniLoginRawResponse = await httpClient.PostAsync("api/Portal/Login", stringContent);
                soltaniLoginRawResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new DomainLogicException("Retriving Login token from api failed", ex);
            }
            SoltaniLoginResponse soltaniLoginResponse = JsonConvert.DeserializeObject<SoltaniLoginResponse>(await soltaniLoginRawResponse.Content.ReadAsStringAsync());


            Customer customer = await CustomersRepository.GetByIdAsync(cancellationToken, Guid.Parse(UserInformationProvider.GetCurrentUserId()));
            SoltaniFindExpertRequest soltaniFindExpertParams = new SoltaniFindExpertRequest();
            soltaniFindExpertParams.UserID = UserInformationProvider.GetCurrentUserId();
            soltaniFindExpertParams.RequestID = evlRequest.Id.ToString();
            soltaniFindExpertParams.Type = (short)evlRequest.InsuranceType;
            DefaultDateTimeUtils defaultDateTimeUtils = new DefaultDateTimeUtils();
            soltaniFindExpertParams.AccidentDate = defaultDateTimeUtils.ConvertMiladiToShamsi(evlRequest.AccidentDate);
            soltaniFindExpertParams.MapLat = evlRequest.Latitude;
            soltaniFindExpertParams.MapLng = evlRequest.Longitude;
            soltaniFindExpertParams.LostName = evlRequest.LostFirstName;
            soltaniFindExpertParams.LostFamily = evlRequest.LostLastName;
            soltaniFindExpertParams.LostInsuranceID = 1; // 1
            soltaniFindExpertParams.LostCarID = 12608; // 12608
            soltaniFindExpertParams.Address = "یوسف آباد کوچه هفتم";
            HttpRequestMessage findNearExpertRequest = new HttpRequestMessage(HttpMethod.Post, "api/Portal/FindNearExpertTest")
            {
                Content = new StringContent(JsonConvert.SerializeObject(soltaniFindExpertParams), UnicodeEncoding.UTF8, "application/json")
            };
            findNearExpertRequest.Headers.Add("auth", soltaniLoginResponse.token);
            HttpResponseMessage findExpertRawResponse = await httpClient.SendAsync(findNearExpertRequest);
            try
            {
                findExpertRawResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new DomainLogicException("FindNearExpert call failed", ex);
            }
            EvlRequestExpertDto evlRequestExpertDto = JsonConvert.DeserializeObject<EvlRequestExpertDto>(await findExpertRawResponse.Content.ReadAsStringAsync());


            evlRequest.EvlRequestExpert = Mapper.Map<EvlRequestExpert>(evlRequestExpertDto);
            await EvlRequestsRepository.UpdateAsync(evlRequest, cancellationToken);
            EvlRequestExpertExpertDto result = Mapper.Map<EvlRequestExpertExpertDto>(evlRequestExpertDto.Expert);

            evlRequest.EvlRequestExpert.Token = evlRequestExpertDto.Token;
            evlRequest.EvlRequestExpert.FileID = evlRequestExpertDto.FileID;
            evlRequest.EvlRequestExpert.Expert.ID = result.ID;
            evlRequest.EvlRequestExpert.Expert.ExpertID = result.ExpertID;
            evlRequest.EvlRequestExpert.Expert.Name = result.Name;
            evlRequest.EvlRequestExpert.Expert.Mobile = result.Mobile;
            //evlRequest.EvlRequestExpert.Expert.MapLat = result.MapLat;
            //evlRequest.EvlRequestExpert.Expert.MapLng = result.MapLng;
            evlRequest.EvlRequestExpert.Expert.Photo = result.Photo;
            evlRequest.EvlRequestExpert.Token = evlRequestExpertDto.ExpertDistance;
            EvlRequestsRepository.Update(evlRequest);

            return result;
        }

    }

}
