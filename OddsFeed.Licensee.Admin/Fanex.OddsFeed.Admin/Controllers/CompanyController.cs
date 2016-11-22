// --------------------------------------------------------------------------
// <copyright file="CompanyController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.03.26">Created</change>
// </history>

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Criteria.CompanyIP;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using System.Data;
    using System.Web.Mvc;

    /// <summary>
    /// Class CompanyController.
    /// </summary>
    [IsAuthorize]
    public class CompanyController : BaseController
    {
        #region Members

        /// <summary>
        /// The _service.
        /// </summary>
        private readonly IBaseService _service;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        public CompanyController()
        {
            _service = _service ?? new BaseService { ServiceContext = ServiceContext };
        }

        #endregion

        /// <summary>
        /// Lists the company IP.
        /// </summary>
        /// <returns>The company IP page.</returns>
        [HttpGet]
        public ActionResult ListCompanyIP()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var criteria = new InsertCompanyIPsCriteria();
            var companyIPs = _service.GetDataSet(new GetAllCompanyIPCriteria());
            var listIPFrom = string.Empty;
            var listIPTo = string.Empty;
            var listDescription = string.Empty;
            foreach (DataRow row in companyIPs.Tables[0].Rows)
            {
                listIPFrom += row["IPFrom"].ToString() + ",";
                listIPTo += row["IPTo"].ToString() + ",";
                listDescription += row["Description"].ToString() + '\n';
            }

            criteria.ListIPFrom = listIPFrom.Trim(',');
            criteria.ListIPTo = listIPTo.Trim(',');
            criteria.ListDescription = listDescription.Trim(',');

            return View(criteria);
        }

        /// <summary>
        /// Edits the company i ps.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        public int EditCompanyIPs(InsertCompanyIPsCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _service.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Edits the company IP setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        [HttpPost]
        public int EditCompanyIPSetting(UpdateCompanyIPCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.Description = string.IsNullOrEmpty(criteria.Description) ? string.Empty : criteria.Description;
            var result = _service.UpdateData(criteria);
            return (int)result.Code;
        }

        /// <summary>
        /// Deletes the company IP setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content for deleting.</returns>
        [HttpPost]
        public JsonResult DeleteCompanyIPSetting(DeleteCompanyIPCriteria criteria)
        {
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            var result = this._service.UpdateData(criteria);
            if (result.Code > 0)
            {
                deleteResult.IsSuccess = false;
                deleteResult.ErrorCode = result.Code;
                deleteResult.ResultMessage = result.Message;
            }
            else
            {
                deleteResult.IsSuccess = true;
                deleteResult.ResultMessage = ViewResources.ActionResult_Success_DeleteCompanyIPSetting;
            }

            return Json(deleteResult, JsonRequestBehavior.AllowGet);
        }
    }
}