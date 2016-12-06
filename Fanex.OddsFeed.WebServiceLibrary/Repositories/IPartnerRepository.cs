namespace Fanex.OddsFeed.WebServiceLibrary.Repositories
{
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebServiceLibrary.Criteria;

    public interface IPartnerRepository
    {
        AuthenticatedPartner GetAuthenticatedPartner(CheckPermissionCriteria criteria);
    }
}
