using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync();

        Task<T> AddAsync(T dto);

        Task<T> UpdateAsync(T dto);
    }
}
