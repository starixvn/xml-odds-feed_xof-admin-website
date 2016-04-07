// --------------------------------------------------------------------------
// <copyright file="LicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class LicenseeService.
    /// </summary>    
    public class LicenseeService : BaseService, ILicenseeService
    {
        #region members

        private readonly ILicenseeRepository _licenseeRepository;

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseeService"/> class.
        /// </summary>
        public LicenseeService()
        {
            _licenseeRepository = _licenseeRepository ?? new LicenseeRepository();
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets the current odds style by system identifier.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>Object System.String.</returns>
        public string GetCurrentOddsStyleBySystemId(int systemId)
        {
            try
            {
                var criteria = new GetOddStyleByLicenseeIDCriteria
                {
                    SystemID = systemId
                };

                if (criteria.IsValid())
                {
                    return BaseRepository.ReturnSingleValue(criteria).ToString();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object ICommandResult.</returns>
         public ICommandResult Update(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return this._licenseeRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

         /// <summary>
         /// Gets the active by system identifier.
         /// </summary>
         /// <param name="systemId">The system identifier.</param>
         /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
         public bool GetActiveBySystemId(int systemId)
         {
             try
             {
                 var criteria = new GetActivateByLicenseeIDCriteria
                 {
                     SystemID = systemId
                 };

                 if (criteria.IsValid())
                 {
                     return bool.Parse(BaseRepository.ReturnSingleValue(criteria).ToString());
                 }
             }
             catch (Exception ex)
             {
                 HandleException(ex);
             }

             return false;
         }

         /// <summary>
         /// Gets the profile all partners by system identifier.
         /// </summary>
         /// <param name="systemId">The system identifier.</param>
         /// <returns>Object PartnersProfile.</returns>
         public PartnersProfile GetProfileAllPartnersBySystemId(int systemId)
         {             
             try
             {
                 var criteria = new GetProfileAllPartnersBySystemIdCriteria
                 {
                     SystemID = systemId
                 };

                 if (criteria.IsValid())
                 {
                     return _licenseeRepository.GetProfileAllPartnersBySystemId(criteria);
                 }
             }
             catch (Exception ex)
             {
                 HandleException(ex);
             }

             return null;
         }

        #endregion
    }
}