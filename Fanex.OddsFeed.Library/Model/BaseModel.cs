// --------------------------------------------------------------------------
// <copyright file="BaseModel.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class BaseModel.
    /// </summary>
    public abstract class BaseModel
    {       
        public BaseModel(string pluralName = null)
        {
            PluralName = string.IsNullOrEmpty(pluralName) ? GetType().Name + "s" : pluralName;
        }

        public static string PluralName { get; set; }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <exception cref="System.Exception">This type does not contain property ' + name + '.</exception>
        public void SetPropertyValue(string name, object value)
        {
            var prop = this.GetType().GetProperty(name);
            if (prop == null)
            {
                throw new Exception("This type does not contain property '" + name + "'");
            }

            if (value is DBNull)
            {                
                value = null;
            }

            if (value is DateTime)
            {
                var date = (DateTime)value;                
                value = date.AddHours(4).ToString("yyyy-MM-dd hh:mm:ss");
            }

            prop.SetValue(this, value, null);
        }

        public object GetPropertyValue(string name)
        {
            var prop = this.GetType().GetProperty(name);
            if (prop == null)
            {
                throw new Exception("This type does not contain property '" + name + "'");
            }

            return prop.GetValue(this, null);
        }

        public string[] GetPublicPropertiesName()
        {
            var t = this.GetType();
            var properties = t.GetProperties();
            return (from p in properties
                    select p.Name).ToArray();
        }
    }
}
