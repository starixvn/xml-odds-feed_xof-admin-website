// --------------------------------------------------------------------------
// <copyright file="DomainRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class DomainRepository.
    /// </summary>
    public class DomainRepository : RepositoryBase<DomainSetting>, IDomainRepository
    {
        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {DomainSetting}.</returns>
        public override IEnumerable<DomainSetting> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<DomainSetting>(criteria.GetSpParams()).OrderBy(s => s.Name).ToList<DomainSetting>();
                }
            }
            catch
            {
                throw;
            }
        }

        public DataCollection GetAllDomain(ICriteria criteria)
        {
            var res = new DataCollection();
            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());
            res.Data = db.ExecuteDataSet();
            res.TotalItem = (int)db.GetOuputParams()["rowcount"];
            return res;
        }

        public DataCollection GetDomainSetting(ICriteria criteria)
        {
            var res = new DataCollection();
            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());
            res.Data = db.ExecuteDataSet();
            res.TotalItem = (int)db.GetOuputParams()["rowcount"];
            return res;
        }

        #region IDomainRepository Members

        public int CreateDomain(ICriteria criteria)
        {
            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());
            db.ExecuteNonQuery();

            return (int)db.GetOuputParams()["errcode"];
        }

        public int UpdateDomain(ICriteria criteria)
        {
            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());
            db.ExecuteNonQuery();

            return (int)db.GetOuputParams()["errcode"];
        }

        public int DeleteDomain(ICriteria criteria)
        {
            var db = new ObjectDb(criteria.GetSettingKey());
            db.AssignParamValues(criteria.GetSpParams());
            db.ExecuteNonQuery();

            return (int)db.GetOuputParams()["errcode"];
        }

        #endregion
    }
}
