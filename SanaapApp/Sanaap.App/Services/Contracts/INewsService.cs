using Sanaap.App.ItemSources;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface INewsService
    {
        Task<List<NewsItemSource>> GetNews(FilterNewsDto filterNewsDto);

        Task<NewsItemSource> GetNewsById(int id);
    }
}
