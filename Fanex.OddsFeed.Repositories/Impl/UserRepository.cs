namespace Fanex.OddsFeed.Repositories.Impl
{
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Repositories.Interface;

    public class UserRepository : BaseRepository, IUserRepository
    { 
        public async Task<User> UserLogin(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return await db.QueryEntityAsync<User>(criteria);
            }
        }

        public async Task<PermissionFlag> GetProfile(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return await db.QueryEntityAsync<PermissionFlag>(criteria);
            }
        }

        public async Task UserReCheckIn(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(criteria);
            }
        }
    }
}