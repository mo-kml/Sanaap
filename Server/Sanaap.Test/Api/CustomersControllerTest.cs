using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sanaap.Api.Controllers;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Threading.Tasks;

namespace Sanaap.Test.Api
{
    [TestClass]
    public class CustomersControllerTest
    {
        [TestMethod]
        public async Task RegisterAndLoginTest()
        {
            using (SanaapTestEnvironment testEnvironment = new SanaapTestEnvironment())
            {
                IODataClient oDataClient = testEnvironment.Server.BuildODataClient(odataRouteName: "Sanaap");

                CustomerDto customer = new CustomerDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Mobile = 9124659995,
                    NationalCode = 1270345565
                };

                int otp = await oDataClient.Controller<CustomersController, CustomerDto>()
                    .Action(nameof(CustomersController.RegisterCustomer))
                    .Set(new CustomersController.RegisterCustomerArgs
                    {
                        customer = customer
                    })
                    .ExecuteAsScalarAsync<int>();

                TokenResponse token = await testEnvironment.Server.Login(customer.NationalCode.ToString(), otp.ToString(), "SanaapResOwner", "secret");

                Assert.IsFalse(token.IsError);

                oDataClient = testEnvironment.Server.BuildODataClient(odataRouteName: "Sanaap", token: token);

                CustomerDto customer2 = await oDataClient.Controller<CustomersController, CustomerDto>()
                    .Function(nameof(CustomersController.GetCurrentCustomer))
                    .FindEntryAsync();

                Assert.AreEqual(customer.FirstName, customer2.FirstName);
                Assert.AreEqual(customer.LastName, customer2.LastName);
                Assert.AreEqual(customer2.IsActive, true);
            }
        }
    }
}
