// --------------------------------------------------------------------------
// <copyright file="CriteriaBase.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.21">Created</change>
//   <change who="Ricky.Nguyen" date="2012.6.25">Update Get Property DbParamaters and SPParam Attribute</change>
//   <change who="Anders.Le" date="2012.6.25">Change property to method</change>
//   <change who="Thai.Tran" date="2013.03.04">Check key existed </change>
// </history>

namespace Fanex.OddsFeed.Framework.Data
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Fanex.Data.Repository;

    /// <summary>
    /// Class CriteriaBase.
    /// </summary>
    public abstract class PageCriteriaBase : Fanex.Data.Repository.ICriteria
    {
        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageCount { get; set; }

        [SpParam(Ignored = true)]
        public int RowCount { get; set; }

        /// <summary>
        /// Gets store procedure parameters.
        /// </summary>
        /// <returns>The hash table contains parameters of store procedure name.</returns>
        public virtual Hashtable GetSpParams()
        {
            var storeProcNameParams = new Hashtable();
            var type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                var typeOfPropertyAttribute = typeof(SpParamAttribute);
                var attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo, typeOfPropertyAttribute);

                if (attr == null)
                {
                    storeProcNameParams.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                }
                else if (attr != null
                    && !attr.Ignored
                    && attr.SqlParamName != null)
                {
                    storeProcNameParams.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                }
            }

            return storeProcNameParams;
        }

        /// <summary>
        /// Add store procedure parameters.
        /// </summary>
        /// <param name="storeProcParamName">The store procedure name.</param>
        /// <returns>The hash table contains parameters of store procedure name.</returns>
        public virtual Hashtable AddSpParams(Hashtable storeProcParamName)
        {
            var type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                var typeOfPropertyAttribute = typeof(SpParamAttribute);
                var attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo, typeOfPropertyAttribute);

                if (attr == null)
                {
                    if (!storeProcParamName.Contains(propertyInfo.Name.ToLower()))
                    {
                        storeProcParamName.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                    }
                }
                else if (attr != null
                    && !attr.Ignored
                    && attr.SqlParamName != null)
                {
                    if (!storeProcParamName.Contains(attr.SqlParamName.ToLower()))
                    {
                        storeProcParamName.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                    }
                }
            }

            return storeProcParamName;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
