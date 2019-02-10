using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Contracts
{
    public interface IExternalApiService
    {
        Task<IEnumerable<ExternalEntityDto>> GetColors();

        Task<IEnumerable<ExternalEntityDto>> GetCars();

        Task<IEnumerable<InsurerDto>> GetInsurers();

        Task<IEnumerable<ContentDto>> GetNews(FilterNewsDto filterNewsDto);

        Task<ContentDto> GetNewsById(int id);

        Task<bool> LikeNews(int id);

        Task<IEnumerable<ExternalEntityDto>> GetNumberplateAlphabets();

    }
}
