using Sanaap.App.ItemSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface INewsService
    {
        Task<List<NewsItemSource>> GetNews();

        Task<NewsItemSource> GetNewsById(int id);
    }
}
