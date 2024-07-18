namespace Fanex.OddsFeed.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Resources;

    [Serializable]
    public class User
    {
        public int Row { get; set; }
        public int UserId { get; set; }
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

        public DateTime CreateDate { get; set; }

        [Display(Name = "Modifier", ResourceType = typeof(ViewResources))]
        public User Modifier { get; set; }

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

        public string RoleName { get; set; }

        [Display(Name = "Active", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }
        public bool IsIgnoreOTP { get; set; }
        public bool IsDeleted { get; set; }

        public string Creator { get; set; }
    }
}