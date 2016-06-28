// --------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
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
    public interface IUserRepository : IRepository<IUser>
    {
        IUser UserLogin(ref UserLoginUserCriteria criteria);

        PermissionFlag GetProfile(ref UserGetProfileCriteria criteria);

        PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria);   

        void UserReCheckIn(ref UserReCheckInUserCriteria criteria);

        void UserLogOut(LogOutUserCriteria criteria);

        IQueryable<dynamic> GetUserInfo(GetUserInfoCriteria criteria);
    }
}