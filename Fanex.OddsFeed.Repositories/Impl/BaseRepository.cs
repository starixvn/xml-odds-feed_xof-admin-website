namespace Fanex.OddsFeed.Repositories.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Repositories.Interface;

    public class BaseRepository : IBaseRepository
    {
        public async Task<DataGrid<T>> GetDataGrid<T>(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var result = new DataGrid<T>
                {
                    Data = (await db.QueryAsync<T>(criteria)).ToList()
                };

                var totalItems = db.GetOutputParams()["ROWCOUNT"];
                result.TotalItems = (totalItems is DBNull) ? 0 : Convert.ToInt32(totalItems);

                return result;
            }
        }
    }
}