// --------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.15">Created</change>
//   <change who="Thai.Tran" date="2013.03.04">Add "ReturnICommandResult", "ReturnDataReader", "ReturnDataCollection"</change>
// </history>
namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <typeparam name="T">The type T.</typeparam>    
    public class RepositoryBase<T> : ReadonlyRepository<T>, IBaseRepository
    {
        /// <summary>
        /// Gets the command result.
        /// </summary>
        /// <param name="hash">The hash table.</param>
        /// <returns>Command Result.</returns>
        public CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];

            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }

        /// <summary>
        /// Returns the single value.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System Object.</returns>
        public object ReturnSingleValue(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(hash);
            return db.ExecuteScalar();
        }

        /// <summary>
        /// Returns the i command result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult ReturnICommandResult(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Returns the i command result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult ReturnICommandResult(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Returns the data reader.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>IData Reader.</returns>
        public IDataReader ReturnDataReader(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(hash);
            return db.ExecuteReader();
        }

        /// <summary>
        /// Returns the data reader.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>IData Reader.</returns>
        public IDataReader ReturnDataReader(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(hash);
            return db.ExecuteReader();
        }

        /// <summary>
        /// Returns the data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>The DataSet.</returns>
        public DataSet ReturnDataSet(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(hash);
            return db.ExecuteDataSet();
        }

        /// <summary>
        /// Returns the data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>The DataSet.</returns>
        public DataSet ReturnDataSet(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(hash);
            return db.ExecuteDataSet();
        }

        /// <summary>
        /// Returns the data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>Data Collection.</returns>
        public DataCollection ReturnDataCollection(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            var res = new DataCollection();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];

                return res;
            }
        }

        /// <summary>
        /// Returns the data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>Data Collection.</returns>
        public DataCollection ReturnDataCollection(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            var res = new DataCollection();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];

                return res;
            }
        }

        /// <summary>
        /// Updates the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Updates the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Update(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Deletes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Deletes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Delete(ICriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        public ICommandResult Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(T entity)
        {
            throw new NotImplementedException();
        }
      
        public ICommandResult Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}