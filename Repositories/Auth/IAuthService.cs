using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Common;
using PD_Store.ViewModels.Auth;

namespace PD_Store.Repositories.Auth
{
    public interface IAuthService
    {
        public Task<DataResult<string>> Register(RegisterRequestVM register, string returnUrl);

        public Task<DataResult<string>> Login(LoginRequestVM login, string returnUrl);

    }
}