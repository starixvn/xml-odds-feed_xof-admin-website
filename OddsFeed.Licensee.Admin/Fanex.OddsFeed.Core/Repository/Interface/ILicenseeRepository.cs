// --------------------------------------------------------------------------
// <copyright file="ILicenseeRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ILicenseeRepository : IBaseRepository
    {
        PartnersProfile GetProfileAllPartnersBySystemId(ICriteria criteria);
        ////IEnumerable<ILicensee> GetLicenseeByPage(ICriteria criteria);
        ////DataSet GetLicenseeByPageReturnDataSet(ICriteria criteria);
        ////IDataReader GetLicenseeByPageReturnDataReader(ICriteria criteria);
        ////int CreateLicensee(ICriteria criteria);
        ////int UpdateLicensee(ICriteria criteria);
        ////int DeleteLicensee(ICriteria criteria);
    }
}