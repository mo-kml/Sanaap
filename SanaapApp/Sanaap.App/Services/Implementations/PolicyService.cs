﻿using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Sanaap.Enums;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class PolicyService : DefaultService<InsurancePolicyDto>, IPolicyService
    {
        private readonly IODataClient _oDataClient;
        private readonly HttpClient _httpClient;
        private readonly IInitialDataService _initialDataService;
        private readonly ILicenseHelper _licenseHelper;
        public PolicyService(HttpClient httpClient, IODataClient oDataClient, IInitialDataService initialDataService, ILicenseHelper licenseHelper) : base(oDataClient)
        {
            _oDataClient = oDataClient;
            _httpClient = httpClient;
            _initialDataService = initialDataService;
            _licenseHelper = licenseHelper;
        }

        public async Task<List<PolicyItemSource>> LoadAllInsurances()
        {
            IEnumerable<InsurancePolicyDto> insurances = (await _oDataClient.For<InsurancePolicyDto>(controllerName)
                .Function("LoadInsurances")
                .OrderBy(it => it.CreatedOn)
                .FindEntriesAsync());

            return await ConvertPolicyToItemSource(insurances);
        }

        public async Task<List<PolicyItemSource>> LoadAllInsurancesByType(InsuranceType insuranceType)
        {
            IEnumerable<InsurancePolicyDto> insurances = (await _oDataClient.For<InsurancePolicyDto>(controllerName)
                .Function("LoadInsurancesByType")
                .Set(new { insuranceType })
                .OrderBy(it => it.CreatedOn)
                .FindEntriesAsync());

            insurances = insurances.Where(i => i.ExpirationDate >= DateTimeOffset.Now.AddMonths(-2));

            return await ConvertPolicyToItemSource(insurances);
        }

        public async Task<List<PolicyItemSource>> ConvertPolicyToItemSource(IEnumerable<InsurancePolicyDto> insurances)
        {
            List<ExternalEntityDto> cars = new List<ExternalEntityDto>();
            List<ExternalEntityDto> colors = new List<ExternalEntityDto>();
            List<InsurersItemSource> insureres = new List<InsurersItemSource>();
            List<PolicyItemSource> policyItemSource = new List<PolicyItemSource>();

            Guid customerId = (await _initialDataService.GetCurrentUserInfo()).Id;

            cars = (await _initialDataService.GetCars()).ToList();

            colors = (await _initialDataService.GetColors()).ToList();

            insureres = (await _initialDataService.GetInsurers()).ToList();

            foreach (InsurancePolicyDto insurance in insurances)
            {
                PolicyItemSource policy = new PolicyItemSource
                {
                    CarId = insurance.CarId,
                    InsuranceType = insurance.InsuranceType,
                    ChasisNo = insurance.ChasisNo,
                    ColorId = insurance.ColorId,
                    Id = insurance.Id,
                    InsurerNo = insurance.InsurerNo,
                    InsurerId = insurance.InsurerId,
                    PlateNumber = insurance.PlateNumber,
                    VIN = insurance.VIN,
                    ExpirationDate = insurance.ExpirationDate,
                    ColorName = colors.FirstOrDefault(c => c.PrmID == insurance.ColorId)?.Name,
                    CarName = cars.FirstOrDefault(c => c.PrmID == insurance.CarId)?.Name,
                    Image = insureres.FirstOrDefault(c => c.ID == insurance.InsurerId)?.Photo,
                    IsExpired = insurance.ExpirationDate <= DateTimeOffset.Now.AddMonths(1)
                };

                policy.LicensePlateItemSource = _licenseHelper.ConvertToItemSource(policy.PlateNumber);

                policyItemSource.Add(policy);
            }

            return policyItemSource;
        }

    }
}
