using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarberBoss.Models
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}