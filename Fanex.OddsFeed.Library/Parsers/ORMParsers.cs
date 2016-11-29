// --------------------------------------------------------------------------
// <copyright file="ORMParsers.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Data;    
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Xml.Linq;
    using Fanex.OddsFeed.Library.Model;

    public class ORMParsers
    {
        private static JavaScriptSerializer _serializer  = new JavaScriptSerializer();

        static ORMParsers()
        {
            _serializer.MaxJsonLength = int.MaxValue;            
        }

        public static IList<T> TableToList<T>(DataTable table) where T : BaseModel
        {            
            var list = new List<T>();
            foreach (DataRow item in table.Rows)
            {                
                var t = (T)Activator.CreateInstance(typeof(T));
                foreach (DataColumn col in table.Columns)
                {
                    t.SetPropertyValue(col.ColumnName, item[col]);
                }                

                list.Add(t);
            }

            return list;
        }

        public static string BuildJSon<T>(string rootName, IList<T> items) where T : BaseModel
        {
            return _serializer.Serialize(items);            
        }

        public static XContainer BuildXDocument<T>(string rootName, IList<T> items) where T : BaseModel
        {            
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", null));

            var root = new XElement(rootName);
            var childName = typeof(T).Name;
            doc.Add(root);
            if (items.Count > 0)
            {
                var names = items[0].GetPublicPropertiesName();
                foreach (var item in items)
                {
                    var e = new XElement(childName);
                    foreach (var propName in names)
                    {
                        var obj = item.GetPropertyValue(propName);

                        if (obj is string)
                        {
                            e.Add(new XAttribute(propName, Encode(obj)));
                        }
                        else
                        {
                            e.Add(new XAttribute(propName, Convert.ToString(obj)));
                        }
                    }

                    root.Add(e);
                }
            }

            return root;
        }

        private static string Encode(object obj)
        {
            return _serializer.Serialize(obj).Trim('"');
        }
    }
}