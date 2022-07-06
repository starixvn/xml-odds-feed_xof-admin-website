namespace Fanex.OddsFeed.Core.Domain
{
    using System;
    using Fanex.OddsFeed.Framework.Domain;

    public interface IAffiliate : IEntity<int, string>
    {
        short SystemID { get; set; }

        short ProfileID { get; set; }

        string Site { get; set; }

        bool IsAllowed { get; set; }

        string Remark { get; set; }

        DateTime CreatedDate { get; set; }

        DateTime LastUpdate { get; set; }

        DateTime LastRequest { get; set; }

        string Creator { get; set; }

        string ContactPerson { get; set; }

        string ContactEmail { get; set; }
    }
}