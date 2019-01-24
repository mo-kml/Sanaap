using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Contracts
{
    public interface IExternalApiService
    {
        Task<IEnumerable<ExternalEntityDto>> GetColors();

        Task<IEnumerable<ExternalEntityDto>> GetCars();

        Task<IEnumerable<InsurerDto>> GetInsurers();

        Task<IEnumerable<ContentDto>> GetNews();

        Task<ContentDto> GetNewsById(int id, Guid userId);

    }
}
