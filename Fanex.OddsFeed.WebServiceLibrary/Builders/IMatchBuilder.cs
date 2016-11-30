namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Common.DTO;

    public interface IMatchBuilder
    {
        XmlNode CreateMatchesNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Match> deadMatches,
            IEnumerable<Match> liveMatches,
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            bool isSample);
    }
}
