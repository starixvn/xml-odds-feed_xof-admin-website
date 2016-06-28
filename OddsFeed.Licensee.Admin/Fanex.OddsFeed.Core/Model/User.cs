// --------------------------------------------------------------------------
// <copyright file="User.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.11">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Web;

    /// <summary>
    /// Class User.
    /// </summary>
    [Serializable]
    public class User : IUser
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        public string Name { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewResources))]
        public string Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        [Display(Name = "Last_Login_Date", ResourceType = typeof(ViewResources))]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "Created_Date", ResourceType = typeof(ViewResources))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modifier", ResourceType = typeof(ViewResources))]
        public IUser Modifier { get; set; }

        [Display(Name = "Modified_Date", ResourceType = typeof(ViewResources))]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        public string Email { get; set; }

        [Display(Name = "RoleID", ResourceType = typeof(ViewResources))]
        public byte RoleID { get; set; }

        [Display(Name = "Active", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        public string Creator { get; set; }
    }
}