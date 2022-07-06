namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Data;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;

    public interface IAdministratorService
    {
        ICommandResult InsertAdmin(InsertAdminCriteria criteria);

        DataCollection GetAdminListReturnDataCollection(GetAdminCriteria criteria);

        DataSet GetExistedAdminDataSet(GetExistedAdminCriteria criteria);

        DataSet GetTop10AdminReturnDataSet(GetTop10AdminCriteria criteria);

        ICommandResult UpdateAdmin(CriteriaBase criteria);

        ICommandResult UpdateAdminInfo(UpdateAdminInfoCriteria criteria);

        ICommandResult UpdateAdminSystem(UpdateAdminSystemCriteria criteria);

        ICommandResult UpdateAdminPassword(UpdateAdminPasswordCriteria criteria);

        DataSet AdminList_GetTop10(AdminList_GetTop10Criteria criteria);

        DataCollection GetAdminListReturnDataCollection(CheckExistAdministratorCriteria criteria);
    }
}