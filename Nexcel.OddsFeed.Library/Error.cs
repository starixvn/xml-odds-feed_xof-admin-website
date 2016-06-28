// --------------------------------------------------------------------------
// <copyright file="Error.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library
{
    using System.Configuration;
    using System.Xml;
    using System.Xml.Linq;
    using Fanex.Logging;
    
    public struct Error
    {
        private int _id;
        private string _message;

        public Error(int id, string message)
        {
            _id = id;
            _message = message;
        }

        public int Id
        {
            get { return _id; }
        }

        public string Message
        {
            get { return _message; }
        }

        public XmlElement ToXmlElement()
        {
            var xdoc = new XmlDocument();
            var e = xdoc.CreateElement("Error");

            e.SetAttribute("id", _id.ToString());
            e.SetAttribute("message", _message);
            return e;
        }
        
        public XElement ToXElement()
        {            
            var root = new XElement("Error", new XAttribute("id", _id.ToString()), new XAttribute("message", _message));

            return root;
        }      
    } 
}
