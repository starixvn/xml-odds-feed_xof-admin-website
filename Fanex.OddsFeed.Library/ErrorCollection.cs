// --------------------------------------------------------------------------
// <copyright file="ErrorCollection.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using System.Xml;

    /// <summary>
    /// Class ErrorCollection.
    /// </summary>
    public static class ErrorCollection
    {
        private static XmlDocument xdoc = new XmlDocument();

        public static XmlElement UnAuthenticated()
        {
            var e = xdoc.CreateElement("Error");

            e.SetAttribute("id", "1");
            e.SetAttribute("message", "You are not authorized to take this action");

            return e;
        }

        public static XmlElement ParameterSufficent()
        {
            var e = xdoc.CreateElement("Error");

            e.SetAttribute("id", "2");
            e.SetAttribute("message", "Missing parameter!");
            return e;
        }

        public static XmlElement WrongUrlFormat()
        {
            var e = xdoc.CreateElement("Error");

            e.SetAttribute("id", "3");
            e.SetAttribute("message", "Invalid URL");
            return e;
        }

        /// <summary>
        /// Generates content for under maintenance down time.
        /// </summary>
        /// <param name="time">The down time.</param>
        /// <returns>Xml Element.</returns>
        public static XmlElement UMDownTime(string time)
        {
            var e = xdoc.CreateElement("Error");
            var errMsg = new string[]
                                    {
                                        "We are having server maintenance, website will be unavailable until ",
                                        time,
                                        " (HK Time GMT +8). Sorry for the inconveniences caused!"
                                    };

            e.SetAttribute("message", string.Join(null, errMsg));
            return e;
        }
    }
}
