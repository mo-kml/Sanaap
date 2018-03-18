using Bit.Test;
using Bit.Test.Core.Implementations;
using FakeItEasy;
using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sanaap.Api.Controllers;
using Sanaap.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using Sannap.Data.Implementations;
using Simple.OData.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Test.Api
{
    [TestClass]
    public class ExpertsControllerTests
    {
        [TestMethod]
        public async Task DeactivateExpertByIdMustThrowAnExceptionInCaseOfAlreadyDeactivatedExpert()
        {
            using (SanaapTestEnvironment testEnvironment = new SanaapTestEnvironment(new TestEnvironmentArgs
            {
                AdditionalDependencies = dependencyManager =>
                {
                    IUsersRepository usersRepository = A.Fake<UsersRepository>();

                    A.CallTo(() => usersRepository.GetUserByUserNameAndPassword(A<string>.Ignored, A<string>.Ignored, A<CancellationToken>.Ignored))
                        .ReturnsLazily(async valueProvider => new User { Id = Guid.NewGuid(), Password = valueProvider.GetArgument<string>("password"), UserName = valueProvider.GetArgument<string>("userName") });

                    dependencyManager.RegisterInstance(usersRepository);

                    IExpertsRepository expertsRepository = A.Fake<ExpertsRepository>();

                    A.CallTo(() => expertsRepository.GetByIdAsync(A<CancellationToken>.Ignored, A<Guid>.Ignored))
                        .ReturnsLazily(async valueProvider => new Expert { IsActive = false });

                    dependencyManager.RegisterInstance(expertsRepository);
                }
            }))
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", "Sanaap-ResOwner", "secret");

                IODataClient odataClient = testEnvironment.Server.BuildODataClient(odataRouteName: "Sanaap", token: token);

                Guid expertId = Guid.NewGuid();

                try
                {

                    await odataClient.Controller<ExpertsController, ExpertDto>()
                         .Action(nameof(ExpertsController.DeactivateExpertById))
                         .Set(new ExpertsController.DeactivateExpertByIdArgs { expertId = expertId })
                         .ExecuteAsync();

                    throw new InvalidOperationException($"${nameof(DeactivateExpertByIdMustThrowAnExceptionInCaseOfAlreadyDeactivatedExpert)} is not working fine");
                }
                catch (WebRequestException ex) when (ex.Response.Contains($"Expert with id {expertId} is already deactivated"))
                {

                }

                ExpertsController expertsController = TestDependencyManager.CurrentTestDependencyManager
                    .Objects.OfType<ExpertsController>()
                    .Single();

                A.CallTo(() => expertsController.DeactivateExpertById(A<ExpertsController.DeactivateExpertByIdArgs>.That.Matches(args => args.expertId == expertId), A<CancellationToken>.Ignored))
                    .MustHaveHappenedOnceExactly();
            }
        }

        [TestMethod]
        public async Task FullNameMappingMustWorkFine()
        {
            using (SanaapTestEnvironment testEnvironment = new SanaapTestEnvironment(new TestEnvironmentArgs
            {
                AdditionalDependencies = dependencyManager =>
                {
                    IUsersRepository usersRepository = A.Fake<UsersRepository>();

                    A.CallTo(() => usersRepository.GetUserByUserNameAndPassword(A<string>.Ignored, A<string>.Ignored, A<CancellationToken>.Ignored))
                        .ReturnsLazily(async valueProvider => new User { Id = Guid.NewGuid(), Password = valueProvider.GetArgument<string>("password"), UserName = valueProvider.GetArgument<string>("userName") });

                    dependencyManager.RegisterInstance(usersRepository);
                }
            }))
            {
                TokenResponse token = await testEnvironment.Server.Login("ValidUserName", "ValidPassword", "Sanaap-ResOwner", "secret");

                IODataClient odataClient = testEnvironment.Server.BuildODataClient(odataRouteName: "Sanaap", token: token);

                Guid someCityId = await odataClient.Controller<CitiesController, CityDto>()
                    .Top(1)
                    .Select(c => c.Id)
                    .FindScalarAsync<Guid>();

                string firstName = Guid.NewGuid().ToString("N");
                string lastName = Guid.NewGuid().ToString("N");

                ExpertDto expert = await odataClient.Controller<ExpertsController, ExpertDto>()
                    .Set(new ExpertDto { FirstName = firstName, LastName = lastName, CityId = someCityId })
                    .InsertEntryAsync();

                Assert.AreEqual($"{firstName} {lastName}", expert.FullName);
            }
        }
    }
}
