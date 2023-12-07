namespace Fanex.OddsFeed.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Criteria;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;

    public class BaseService : IBaseService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BaseService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return await dynamicRepository.FetchAsync<T>(criteria);
            }

            return new List<T>();
        }

        public async Task<T> GetDataEntityAsync<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return await dynamicRepository.GetAsync<T>(criteria);
            }

            return default(T);
        }

        public async Task<CommandResult> ExecuteNonQueryAsync(BaseNonQueryCommand command)
        {
            var commandResult = new CommandResult
            {
                Code = 1,
                Message = ViewResources.InvalidInputData
            };

            if (command != null && command.IsValid())
            {
                await dynamicRepository.ExecuteAsync(command);
                commandResult.Code = command.ErrCode;
                commandResult.Message = command.ErrMsg;
            }

            return commandResult;
        }

        public async Task<DataGrid<T>> GetDataGridAsync<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                var result = new DataGrid<T>
                {
                    Data = (await dynamicRepository.FetchAsync<T>(criteria)).ToList()
                };
                var properties = criteria.GetType().GetProperties()
                    .Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(criteria, null)))
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value);
                if (properties.ContainsKey("rowcount"))
                {
                    result.TotalItems = int.Parse(properties["rowcount"].ToString());
                }

                return result;
            }

            return default(DataGrid<T>);
        }
    }
}