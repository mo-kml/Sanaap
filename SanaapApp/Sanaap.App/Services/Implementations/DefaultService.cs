using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class DefaultService<T> : IService<T> where T : class, ISanaapDto
    {
        private readonly IODataClient _oDataClient;
        protected readonly string controllerName;
        public DefaultService(IODataClient oDataClient)
        {
            _oDataClient = oDataClient;

            controllerName = new Pluralize.NET.Pluralizer().Pluralize(typeof(T).Name.Replace("Dto", string.Empty));
        }

        public virtual async Task<T> AddAsync(T dto)
        {
            return await _oDataClient.For<T>(controllerName)
                                .Set(dto)
                                .InsertEntryAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _oDataClient.For<T>(controllerName)
                .Function("GetAll")
                .FindEntriesAsync();
        }

        public virtual async Task<T> GetByIdAsync()
        {
            return await _oDataClient.For<T>(controllerName)
                .Function("GetById")
                .FindEntryAsync();
        }

        public virtual async Task<T> UpdateAsync(T dto)
        {
            return await _oDataClient.For<T>(controllerName)
                                .Key(dto.Id)
                                .Set(dto)
                                .UpdateEntryAsync();
        }
    }

}
