using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.OddsFeed.Core.Service.Interface
{
    public interface IEntity<TId, TName>
    {
        TId Id
        {
            get;
            set;
        }

        TName Name
        {
            get;
            set;
        }
    }
}
