using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PD_Store.ViewModels.Auth
{
    public class RegisterRequestVM
    {
        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }
    }
}