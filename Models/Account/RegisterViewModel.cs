using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace empty_template.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public String DisplayName { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$",
        ErrorMessage = "Password must be complex")]
        public String Password { get; set; }
        [Compare("Password")]
        public String ConfirmPassword { get; set; }
    }
}
