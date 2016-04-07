// --------------------------------------------------------------------------
// <copyright file="DomainService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class DomainService.
    /// </summary>    
    public class DomainService : BaseService, IDomainService
    {
        #region members

        private readonly IDomainRepository _domainRepository;

        #endregion

        #region ctors

        public DomainService()
        {
            _domainRepository = _domainRepository ?? new DomainRepository();
        }

        #endregion

        #region methods
        public DataCollection GetAllDomain(ICriteria criteria)
        {
            return _domainRepository.GetAllDomain(criteria);
        }

        public DataCollection GetDomainSetting(ICriteria criteria)
        {
            return _domainRepository.GetDomainSetting(criteria);
        }

        public int CreateDomain(ICriteria criteria)
        {
            return _domainRepository.CreateDomain(criteria);
        }

        public int UpdateDomain(ICriteria criteria)
        {
            return _domainRepository.UpdateDomain(criteria);
        }

        public int DeleteDomain(ICriteria criteria)
        {
            return _domainRepository.DeleteDomain(criteria);
        }
        #endregion
    }
}