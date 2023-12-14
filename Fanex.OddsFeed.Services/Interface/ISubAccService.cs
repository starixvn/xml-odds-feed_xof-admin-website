namespace Fanex.OddsFeed.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.SubAccount;
    using Fanex.OddsFeed.Models;

    public interface ISubAccService
    {
        Task<IEnumerable<User>> GetBySystemIdAsync(short systemId);

        Task<PermissionFlag> GetUserPermissionAsync(int userId, short systemId);

        Task<CommandResult> InsertAsync(InsertSubAccCriteria criteria);
    }
}