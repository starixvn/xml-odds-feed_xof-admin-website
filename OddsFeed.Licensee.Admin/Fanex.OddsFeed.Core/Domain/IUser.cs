namespace Fanex.OddsFeed.Core.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Framework.Domain;

    public interface IUser : IEntity<int, string>
    {
        [Display(Name = "Username", ResourceType = typeof(ViewResources))]
        string Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        string Password { get; set; }

        [Display(Name = "Last_Login_Date", ResourceType = typeof(ViewResources))]
        DateTime LastLoginDate { get; set; }

        [Display(Name = "Created_Date", ResourceType = typeof(ViewResources))]
        DateTime CreatedDate { get; set; }

        [Display(Name = "Modifier", ResourceType = typeof(ViewResources))]
        IUser Modifier { get; set; }

        [Display(Name = "Modified_Date", ResourceType = typeof(ViewResources))]
        DateTime ModifiedDate { get; set; }

        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        string Email { get; set; }

        [Display(Name = "RoleID", ResourceType = typeof(ViewResources))]
        byte RoleID { get; set; }

        [Display(Name = "Active", ResourceType = typeof(ViewResources))]
        bool IsActive { get; set; }

        string Creator { get; set; }
    }
}