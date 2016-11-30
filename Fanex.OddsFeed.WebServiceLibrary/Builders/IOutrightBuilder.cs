namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Xml;
    using Common.DTO;

    public interface IOutrightBuilder
    {
        XmlNode CreateOutrightsNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Outright> outrightNodes,
            bool isOutright,
            bool isSample);
    }
}
