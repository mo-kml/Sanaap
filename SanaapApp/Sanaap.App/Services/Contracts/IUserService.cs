using Sanaap.Dto;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IUserService
    {
        Task<CustomerDto> ActiveUser();
    }
}
