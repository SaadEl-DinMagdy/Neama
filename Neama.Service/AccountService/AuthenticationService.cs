using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using Neama.Core.Services.Contract;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.AccountService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IRedisRepository<string> _redis;
        private readonly IConfiguration _configration;

        public AuthenticationService(UserManager<AppUser> userManager , SignInManager<AppUser> signInManger ,IEmailService emailService, IRedisRepository<string> redis ,IConfiguration configration)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _emailService = emailService;
            _redis = redis;
            _configration = configration;
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var userRole = await _userManager.GetRolesAsync(user);

            foreach(var Role in userRole)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, Role));
            }

            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configration["JWT:SecurityKey"]));

            var Token = new JwtSecurityToken
                (
                audience : _configration["JWT:ValidAudience"],
                issuer : _configration["JWT:ValidIssuer"],
                expires : DateTime.UtcNow.AddDays(double.Parse(_configration["JWT:DurationInDay"])),
                claims : authClaim,
                signingCredentials : new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)               
                );

            return  new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<AppUser?> SignInAsync(string Email, string Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null) 
            {
                return null; 
            }

            var Result = await _signInManager.CheckPasswordSignInAsync(user, Password ,false);

            if (!Result.Succeeded)
            {
                return null;
            }

            return user;
        }

        public async Task<AppUser?> SignUpAsync(string name, string Email, string Password)
        {
            var check = await _userManager.FindByEmailAsync(Email);

            if (check != null) 
            {
                var confirmed = await _userManager.IsEmailConfirmedAsync(check);

                if (confirmed == false)
                {
                    await _userManager.DeleteAsync(check);
                }
                else
                {
                    return null;
                }


            }

            var User = new AppUser()
            {
                DisplayName = name,
                Email = Email,
                UserName = Email,
            };

 
            var Result = await  _userManager.CreateAsync(User, Password);
            if (!Result.Succeeded)
            {
                return null;
            }
            await _userManager.AddToRoleAsync(User, AppRoles.User);
            await SendOtpCodeByEmailAsync(Email , _configration["EmailTemplates:RegistrationBody"], TimeSpan.FromMinutes(3));

            return User;
        }

        public async Task<bool> SendOtpCodeByEmailAsync(string Email , string BodyOfEmail , TimeSpan TimeofExpire)
        {
            var user = await  _userManager.FindByEmailAsync(Email);

            if(user == null)
            {
                return false!;
            }
            var youhaveone = await _redis.GetAsync(Email);
            if (youhaveone != null)
            {
                return true;
            }

            var OtpCode = new Random().Next(1000, 9999).ToString();
            await _redis.SetAsync(Email,OtpCode, TimeofExpire);

            

            await _emailService.SendEmailAsync(Email, "Neama", BodyOfEmail+OtpCode);
            return true;
        }

        public async Task<AppUser?> ConfirmEmailAsync(string Email, string Otp)
        {
            var SavedOtp = await _redis.GetAsync(Email); 
            
            if(SavedOtp == Otp)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    user.EmailConfirmed = true;
                }
                else
                {
                    return null;
                }

                var result = await _userManager.UpdateAsync(user);

                if(!result.Succeeded)
                {
                    return null;
                }

                await _redis.DeleteAsync(Email);

                return user;
            }

            return null;
        }

        public async Task<AppUser?> ForgetPasswordAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null  )
            {
                return null;
            }
            var confirmcheck = await _userManager.IsEmailConfirmedAsync(user);
            if(confirmcheck == false)
            {
                return null;
            }


            await SendOtpCodeByEmailAsync(Email, _configration["EmailTemplates:ForgetPasswordBody"],TimeSpan.FromMinutes(10));

            return user;
        }

        public async Task<AppUser?> ResetPasswordAsync(string Email, string NewPassword ,string Otp)
        {
            var SavedOtp = await _redis.GetAsync(Email);

            if (SavedOtp == Otp)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, NewPassword);

                    if (!result.Succeeded)
                    {
                        return null;
                    }
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _redis.DeleteAsync(Email);

                    return user;
                }
                else
                {
                    return null;
                }

                
            }

            return null;
        }
    }
}
