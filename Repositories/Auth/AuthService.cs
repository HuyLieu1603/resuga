using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Common;
using Microsoft.AspNetCore.Identity;
using PD_Store.DbContextFolder;
using PD_Store.Helper;
using PD_Store.Models.Auth;
using PD_Store.ViewModels.Auth;

namespace PD_Store.Repositories.Auth
{
    public class AuthService
    {
        private readonly AdminDbContext _context;

        private readonly ILogger _logger;

        UserManager<ApplicationUser> _userManager;

        SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            AdminDbContext context,
            ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AuthService>();
        }

        //REGISTER
        public async Task<DataResult<string>> Register(RegisterRequestVM register, string returnUrl)
        {
            var result = new DataResult<string>();
            try
            {
                //Check Account existed
                var user = await _userManager.FindByNameAsync(register.UserName);
                if (user != null)
                {
                    result.Status = Helper.Contants.StatusCodeBadRequest;
                    result.Message = "Tài khoản đã tồn tại, vui lòng thử lại";
                    return result;
                }

                var newUser = new IdentityUser { UserName = register.UserName, Email = register.Email };
                var resultUser = await _userManager.CreateAsync((ApplicationUser)newUser, register.Password);

                if (resultUser.Succeeded)
                {
                    await _signInManager.SignInAsync((ApplicationUser)newUser, isPersistent: false);
                }

                result.Status = Contants.StatusCodeSuccessed;
                result.Message = "Đăng ký thành công";
                result.Data = returnUrl ?? "/";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Helper.Contants.StatusCodeInternalServerError;
                result.Message = "Không thể đăng nhập vào hệ thống, vui lòng báo IT Admin";
            }
            return result;
        }

        //LOG IN
        public async Task<DataResult<string>> Login(LoginRequestVM login, string returnUrl)
        {
            var result = new DataResult<string>();
            try
            {
                //Check Account existed
                var user = await _userManager.FindByNameAsync(login.UserName);
                if (user == null)
                {
                    result.Status = Helper.Contants.StatusCodeBadRequest;
                    result.Message = "Tài khoản không tồn tại, vui lòng thử lại";
                    return result;
                }

                //Login
                var loginResult = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
                if (!loginResult.Succeeded)
                {
                    result.Status = Helper.Contants.StatusCodeBadRequest;
                    result.Message = "Mật khẩu không đúng, vui lòng thử lại";
                    return result;
                }
                result.Status = Helper.Contants.StatusCodeSuccessed;
                result.Message = returnUrl;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi đăng nhập:");
                result.Status = Helper.Contants.StatusCodeInternalServerError;
                result.Message = "Không thể đăng nhập vào hệ thống, vui lòng báo IT Admin";
            }
            return result;
        }

        //LOG OUT
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        //RESET PASSWORD
        public async Task<DataResult<string>> ResetPasswordAsync(string username)
        {
            var result = new DataResult<string>();
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$*";
                    var stringChars = new char[12];
                    var random = new Random();
                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }
                    //Generate random password
                    var newPassword = new String(stringChars);
                    string hashedNewPassword = _userManager.PasswordHasher.HashPassword(
                        user,
                        newPassword
                    );

                    user.PasswordHash = hashedNewPassword;
                    //user.DateChangePassword = DateTime.Now;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    result.Status = Helper.Contants.StatusCodeSuccessed;
                    result.Data = newPassword;
                    return result;
                }
                result.Status = Helper.Contants.StatusCodeBadRequest;
                result.Message = "Không thể tìm thấy người dùng trên, vui lòng thử lại";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Helper.Contants.StatusCodeInternalServerError;
                result.Message = "Không thể reset Password, vui lòng liên hệ IT Admin";
            }
            return result;
        }

        //Update status active user
        public async Task<DataResult<bool>> UpdateUserStatus(string username, bool active)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (currentUser == null)
            {
                return new DataResult<bool>
                {
                    Status = Contants.StatusCodeNotFound,
                    Message = "Người dùng không tồn tại"
                };
            }
            currentUser.Active = active;
            currentUser.LastUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return new DataResult<bool>
            {
                Status = Contants.StatusCodeSuccessed,
            };
        }


    }
}