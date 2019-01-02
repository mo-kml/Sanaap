using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sanaap.Api.Controllers;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.Test.Api
{
    [TestClass]
    public class EvlRequestsControllerTests
    {
        [TestMethod]
        public async Task LoginAndSubmitNewEvlRequestAndFindExpertForItTest()
        {
            using (SanaapTestEnvironment testEnv = new SanaapTestEnvironment())
            {
                TokenResponse token = await testEnv.Server.Login("1270340050", "09033610498", "SanaapResOwner", "secret");

                HttpClient httpClient = testEnv.Server.BuildHttpClient(token);

                MultipartFormDataContent content = new MultipartFormDataContent
                {
                    //new StringContent(JsonConvert.SerializeObject(new EvlRequestDto
                    //{
                    //    AccidentDate = DateTimeOffset.UtcNow,
                    //    CompanyId = 1,
                    //    AccidentReason = "Test",
                    //    InsuranceNumber = "123-456",
                    //    InsuranceTypeEnum = InsuranceType.Sales,
                    //    Latitude = 12,
                    //    Longitude = 12,
                    //    VehicleKindId = 1,
                    //    VehicleNumber = "ایران - ب 44 678"
                    //}), Encoding.UTF8, "application/json"),

                    { new StreamContent(File.OpenRead(@"C:\Users\SOFT\Desktop\Mohammad.png")), Guid.Parse("9bbd650e-3415-494d-b382-623a0840ab5a").ToString(), Guid.Parse("9bbd650e-3415-494d-b382-623a0840ab5a").ToString() }
                };

                HttpRequestMessage submitEvlRequest = new HttpRequestMessage(HttpMethod.Post, "api/evl-requests/submit-evl-request")
                {
                    Content = content
                };

                HttpResponseMessage submitEvlRequestExpertResponse = await httpClient.SendAsync(submitEvlRequest);

                submitEvlRequestExpertResponse.EnsureSuccessStatusCode();

                EvlRequestDto evlRequest = JsonConvert.DeserializeObject<EvlRequestDto>(await submitEvlRequestExpertResponse.Content.ReadAsStringAsync());

                IODataClient odataClient = testEnv.Server.BuildODataClient(odataRouteName: "Sanaap", token: token);

                EvlRequestExpertDto evlRequestExpert = await odataClient.Controller<EvlRequestExpertsController, EvlRequestExpertDto>()
                    .Function(nameof(EvlRequestExpertsController.FindEvlRequestExpert))
                    .Set(new { evlRequestId = evlRequest.Id })
                    .FindEntryAsync();

                evlRequestExpert = await odataClient.Controller<EvlRequestExpertsController, EvlRequestExpertDto>()
                    .Function(nameof(EvlRequestExpertsController.FindEvlRequestExpert))
                    .Set(new { evlRequestId = evlRequest.Id })
                    .FindEntryAsync();
            }
        }
    }
}
