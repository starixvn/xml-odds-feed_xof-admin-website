// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;        
    using Fanex.OddsFeed.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IUserService : IBaseService
    {
        IUser UserLogin(ref UserLoginUserCriteria criteria);

        PermissionFlag UserGetProfile(ref UserGetProfileCriteria criteria);

        PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria);

        void ReUserCheckIn(ref UserReCheckInUserCriteria criteria);

        void UserLogOut(LogOutUserCriteria criteria);

        IEnumerable<IUser> GetUserInfo(GetUserInfoCriteria criteria);
    }
}
