namespace Fanex.OddsFeed.Framework.Data
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Fanex.Data.Repository;

    public abstract class PageCriteriaBase : ICriteria
    {
        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageCount { get; set; }

        [SpParam(Ignored = true)]
        public int RowCount { get; set; }

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
                else
                {
                    if (!attr.Ignored && attr.SqlParamName != null)
                    {
                        storeProcNameParams.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                    }
                }
            }

            return storeProcNameParams;
        }

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
                else
                {
                    if (!attr.Ignored && attr.SqlParamName != null && !storeProcParamName.Contains(attr.SqlParamName.ToLower()))
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