// --------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Framework.Data;    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ILicenseeService : IBaseService
    {
        string GetCurrentOddsStyleBySystemId(int systemId);

        ICommandResult Update(CriteriaBase criteria);

        bool GetActiveBySystemId(int systemId);

        PartnersProfile GetProfileAllPartnersBySystemId(int systemId);

        ////IEnumerable<ILicensee> GetLicenseeByPage(GetLicenseeByPageCriteria criteria);
        ////DataSet GetLicenseeByPageReturnDataSet(GetLicenseeByPageCriteria criteria);
        ////IDataReader GetLicenseeByPageReturnDataReader(GetLicenseeByPageCriteria criteria);
        ////int CreateLicensee(CreateLicensee1Criteria criteria);
        ////int UpdateLicensee(EditLicenseeCriteria criteria);
        ////int DeleteLicensee(DeleteLicenseeCriteria criteria);

        ////DataCollection GetAllLicenseeRequestReturnDataCollection(LicenseeRequestCriteria criteria);
        ////DataCollection GetLicenseeRequestDetailReturnDataCollection(LicenseeRequestCriteria criteria);
    }
}