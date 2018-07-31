using AutoMapper;
using Bit.Core.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Newtonsoft.Json;
using Sanaap.App.Dto;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Model;
using Sanaap.Service.Contracts;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestExpertsController : DtoController<EvlRequestExpertDto>
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual ISanaapRepository<EvlRequest> EvlRequestsRepository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestDto, EvlRequest> EvlRequestDtoEntityMapper { get; set; }

        public virtual IMapper Mapper { get; set; }

        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }

        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        [Function]
        public virtual async Task<EvlRequestExpertDto> FindEvlRequestExpert(Guid evlRequestId, CancellationToken cancellationToken)
        {
            EvlRequest evlRequest = await EvlRequestsRepository.GetByIdAsync(cancellationToken, evlRequestId);

            if (evlRequest == null)
                throw new ResourceNotFoundException("evlRequest is null");

            if (!string.IsNullOrEmpty(evlRequest.EvlRequestExpert.Token))
            {
                return Mapper.Map<EvlRequestExpertDto>(evlRequest.EvlRequestExpert);
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
            soltaniFindExpertParams.Type = evlRequest.InsuranceTypeEnum == InsuranceType.Sales ? "3" : "1";
            soltaniFindExpertParams.MapLat = evlRequest.Latitude.ToString();
            soltaniFindExpertParams.MapLng = evlRequest.Longitude.ToString();
            soltaniFindExpertParams.LostName = evlRequest.OwnerFullName != null ? evlRequest.OwnerFullName.Trim() : customer.FirstName;
            soltaniFindExpertParams.LostFamily = evlRequest.OwnerFullName != null ? "" : customer.LastName;
            soltaniFindExpertParams.LostMobile = evlRequest.OwnerMobileNumber != null ? evlRequest.OwnerMobileNumber : customer.Mobile;
            soltaniFindExpertParams.LostInsuranceID = "0";
            soltaniFindExpertParams.LostInsuranceNO = evlRequest.InsuranceNumber;
            soltaniFindExpertParams.LostCarID = "12608";
            soltaniFindExpertParams.LostCarType = "415";
            soltaniFindExpertParams.Address = "";

            HttpRequestMessage findNearExpertRequest = new HttpRequestMessage(HttpMethod.Post, "api/Portal/FindNearExpert")
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

            return Mapper.Map<EvlRequestExpertDto>(evlRequest.EvlRequestExpert);
        }

        public class SoltaniFindExpertRequest
        {
            public string UserID { get; set; }
            public string RequestID { get; set; }
            public string Type { get; set; }
            public string MapLat { get; set; }
            public string MapLng { get; set; }
            public string LostName { get; set; }
            public string LostFamily { get; set; }
            public string LostMobile { get; set; }
            public string LostInsuranceID { get; set; }
            public string LostInsuranceNO { get; set; }
            public string LostCarID { get; set; }
            public string LostCarType { get; set; }
            public string Address { get; set; }
        }
    }
}
