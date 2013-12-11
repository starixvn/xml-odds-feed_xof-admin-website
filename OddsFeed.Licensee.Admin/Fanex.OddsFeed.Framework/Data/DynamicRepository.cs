// --------------------------------------------------------------------------
// <copyright file="DynamicRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
// </history>

namespace Fanex.OddsFeed.Framework.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fanex.Common.Helper;
    using Fanex.Data;

    /// <summary>
    /// Class DynamicRepository.
    /// </summary>
    public class DynamicRepository : IDynamicRepository
    {
        public virtual T Get<T>(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The instant of type T.</returns>
        public virtual T Get<T>(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    T entity = objectDb.QueryEntity<T>(criteria.GetSpParams());
                    return entity;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Counts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The count number.</returns>
        public virtual int Count(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    int count = Convert.ToInt32(objectDb.ExecuteScalar(criteria.GetSpParams()));

                    return count;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<T> Fetch<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable of type T.</returns>
        public virtual IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IEnumerable<T> entityList = objectDb.Query<T>(criteria.GetSpParams());

                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="order">The order.</param>
        /// <returns>IEnumerable of type T.</returns>
        public virtual IEnumerable<T> Fetch<T>(ICriteria criteria, Action<Orderable<T>> order)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IEnumerable<T> entityList = objectDb.Query<T>(criteria.GetSpParams());

                    if (entityList != null && entityList.Any())
                    {
                        var orderable = new Orderable<T>(entityList);
                    }

                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Singles the specified parameters.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The instant of type T.</returns>
        public virtual T Single<T>(dynamic parameters)
        {
            try
            {
                var hash = ConvertDynamicParamsToHashtable(parameters);
                var settingKey = GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    var entity = objectDb.QueryEntity<T>(null, hash);
                    return entity;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Queries the specified parameters.
        /// </summary>
        /// <typeparam name="T">The type T.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEnumerable of type T.</returns>
        public virtual IEnumerable<T> Query<T>(dynamic parameters)
        {
            try
            {
                var hash = ConvertDynamicParamsToHashtable(parameters);
                var settingKey = GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    var entityList = objectDb.Query<T>(null, hash);
                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get length the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The length.</returns>
        public virtual int Length(dynamic parameters)
        {
            try
            {
                var hash = ConvertDynamicParamsToHashtable(parameters);
                var settingKey = GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    return Convert.ToInt32(objectDb.ExecuteScalar(hash));
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Check setting key whether exist in the parameter collection or not.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        protected void CheckSettingKeyExist(dynamic parameters)
        {
            Check.Argument.IsNotNull(parameters, "parameters");

            Type type = parameters.GetType();
            var propInfo = type.GetProperty("SettingKey");

            if (propInfo == null)
            {
                throw new NullReferenceException("Input paramater does not contain the expected property SettingKey");
            }
        }

        /// <summary>
        /// Converts the dynamic parameters to hash table.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Hash table.</returns>
        protected Hashtable ConvertDynamicParamsToHashtable(dynamic parameters)
        {
            CheckSettingKeyExist(parameters);

            Type type = parameters.GetType();
            var hash = new Hashtable();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.Name.Equals("SettingKey"))
                {
                    hash.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(parameters, null));
                }
            }

            return hash;
        }

        /// <summary>
        /// Adds the dynamic parameters to hash table.
        /// </summary>
        /// <param name="hash">The hash table.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The hash table combined with parameters.</returns>
        protected Hashtable AddDynamicParamsToHashtable(Hashtable hash, dynamic parameters)
        {
            Type type = parameters.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.Name.Equals("SettingKey"))
                {
                    hash.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(parameters, null));
                }
            }

            return hash;
        }

        /// <summary>
        /// Gets the setting key from dynamic parameter.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The setting key.</returns>
        protected string GetSettingKeyFromDynamicParam(dynamic parameters)
        {
            Type type = parameters.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.Name.Equals("SettingKey"))
                {
                    return propertyInfo.GetValue(parameters, null);
                }
            }

            return string.Empty;
        }
    }
}
