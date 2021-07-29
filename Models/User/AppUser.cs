using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty_template.Models.User
{
    public class AppUser : IdentityUser
    {
        public String DisplayName { get; set; }
        public String Bio { get; set; }
    }
}
