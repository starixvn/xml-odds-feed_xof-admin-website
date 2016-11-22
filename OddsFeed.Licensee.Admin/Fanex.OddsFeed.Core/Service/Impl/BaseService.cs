// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.14">Add GetRepositoryExecLog</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.Databank.Repositories;
    using Fanex.Logging;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class BaseService.
    /// </summary>    
    public class BaseService : IBaseService
    {        
        private readonly SelectListRepository _selectListRepository;
        private readonly RepositoryBase<IAffiliate> _baseRepository;

        public BaseService()
        {
            this._selectListRepository = this._selectListRepository ?? new SelectListRepository();
            this._baseRepository = this._baseRepository ?? new RepositoryBase<IAffiliate>();
            this.ServiceContext = this.ServiceContext ?? new ServiceContext();
        }

        public IServiceContext ServiceContext { get; set; }

        public RepositoryBase<IAffiliate> BaseRepository 
        {
            get
            {
                return _baseRepository;
            }      
        }

        public void HandleException(Exception ex)
        {
            throw ex;
        }

        /// <summary>
        /// Gets the log execute store procedure criteria.
        /// </summary>
        /// <returns>Object LogExecSPCriteria.</returns>
        public LogExecSpCriteria GetLogExecSpCriteria()
        {
            Check.Argument.IsNotNull(this.ServiceContext, "ServiceContext");

            return new LogExecSpCriteria
            {
                CurrentUserId = this.ServiceContext.UserId,
                CurrentSystemId = this.ServiceContext.SystemId,
                LogDate = DateTime.Now,
                LoginIp = this.ServiceContext.Ip
            };
        }

        public IEnumerable<SelectListItem> GetSelectList(string settingKey, bool order = true, string selectedValue = "")
        {
            return _selectListRepository.Get(settingKey, order, selectedValue);
        }

        public IEnumerable<SelectListItem> GetSelectList(ICriteria criteria, bool order = true, string selectedValue = "")
        {
            return _selectListRepository.Get(criteria, order, selectedValue);
        }

        public DataCollection GetData(ICriteria criteria)
        {
            return this._baseRepository.ReturnDataCollection(criteria);
        }

        public IDataReader GetDataReader(ICriteria criteria)
        {
            return this._baseRepository.ReturnDataReader(criteria);
        }

        public DataSet GetDataSet(ICriteria criteria)
        {
            return this._baseRepository.ReturnDataSet(criteria);
        }

        public ICommandResult UpdateData(ICriteria criteria)
        { 
            return this._baseRepository.Update(criteria);
        }

        public ICommandResult DeleteData(ICriteria criteria)
        {
            return this._baseRepository.Delete(criteria);
        }
    }
}