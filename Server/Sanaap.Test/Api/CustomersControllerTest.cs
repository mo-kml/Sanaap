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
                    Mobile = "09124659995",
                    NationalCode = "1270340050"
                };

                await oDataClient.Controller<CustomersController, CustomerDto>()
                    .Action(nameof(CustomersController.RegisterCustomer))
                    .Set(new CustomersController.RegisterCustomerArgs
                    {
                        customer = customer
                    })
                    .ExecuteAsync();

                TokenResponse token = await testEnvironment.Server.Login(customer.NationalCode, customer.Mobile, "SanaapResOwner", "secret");

                Assert.IsFalse(token.IsError);

                oDataClient = testEnvironment.Server.BuildODataClient(odataRouteName: "Sanaap", token: token);

                CustomerDto customer2 = await oDataClient.Controller<CustomersController, CustomerDto>()
                    .Function(nameof(CustomersController.GetCurrentCustomer))
                    .FindEntryAsync();

                Assert.AreEqual(customer.FirstName, customer2.FirstName);
                Assert.AreEqual(customer.LastName, customer2.LastName);
            }
        }
    }
}
