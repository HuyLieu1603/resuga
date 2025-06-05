using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PD_Store.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string Gender { get; set; } = string.Empty;

        public required string Role { get; set; }

        public override string? Email { get => base.Email; set => base.Email = value; }

        public string? Address { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Active { get; set; } = true;

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {

        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName) { }

        public string? DisplayName { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public required virtual ApplicationUser User { get; set; }

        public required virtual ApplicationRole Role { get; set; }
    }
}