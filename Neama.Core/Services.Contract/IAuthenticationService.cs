using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IAuthenticationService
    {
        Task<string> CreateTokenAsync(AppUser user);
        Task<AppUser?> SignInAsync(string Email,string Password);
        Task<AppUser?> SignUpAsync(string name,string Email , string Password);
        Task<bool> SendOtpCodeByEmailAsync(string Email, string BodyOfEmail, TimeSpan TimeofExpire);
        Task<AppUser?> ConfirmEmailAsync(string Email, string Otp);
        Task<AppUser?> ForgetPasswordAsync(string Email);
        Task<AppUser?> ResetPasswordAsync(string Email , string NewPassword ,string Otp);

    }
}
