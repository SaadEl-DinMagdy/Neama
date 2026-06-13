using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Dtos;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly IAuthenticationService _auth;
        private readonly IConfiguration _configration;


        public AccountController(IAuthenticationService auth , IConfiguration configration )
        {
            _auth = auth;
            _configration = configration;

        }


        [HttpPost("SignIn")]
        public async Task<ActionResult<UserDtos>> SignInAsync(LoginDtos model)
        {
            var user = await _auth.SignInAsync(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(400, "Email Or Password incorrect"));
            }

            return Ok(new UserDtos()
            {
                Name = user.DisplayName,
                Email = user.Email,
                Token = await _auth.CreateTokenAsync(user)
            });

        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<UserNameAndEmailDto>> SignUpAsync(RegisterDtos model)
        {
            var user = await _auth.SignUpAsync(model.Name, model.Email, model.Password);

            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new List<string>() { "This email already exists" } });
            }

            return Ok(new UserNameAndEmailDto()
            {
                Name = user.DisplayName,
                Email = user.Email,
            });
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<UserDtos>> ConfirmEmailAsync(ConfirmDto model)
        {
            var user = await _auth.ConfirmEmailAsync(model.Email, model.Otp);

            if (user == null)
            {
                return BadRequest(new ApiResponse(400, "the Otp Code Wrong"));
            }

            return Ok(new UserDtos()
            {
                Name = user.DisplayName,
                Email = user.Email,
                Token = await _auth.CreateTokenAsync(user)
            });
        }

        [HttpPost("ResendEmail")]
        public async Task<ActionResult<string>> ResendEmailAsync(ResendEmailDto model)
        {
            var result = await _auth.SendOtpCodeByEmailAsync(model.Email , _configration["EmailTemplates:RegistrationBody"], TimeSpan.FromMinutes(3));

            if (!result)
            {
                return BadRequest(new ApiResponse(400,"برجاء فحص بريدك الإلكتروني. إذا لم يصلك، يمكنك المحاولة مرة أخرى بعد قليل."));
            }
            return Ok("تم الارسال");

        }

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<UserNameAndEmailDto>> ForgetPasswordAsync(ResendEmailDto model)
        {
            var user = await _auth.ForgetPasswordAsync(model.Email);

            if(user == null)
            {
                return BadRequest(new ApiResponse(400, "هذا البريد الإلكتروني غير مسجل لدينا."));
            }

            return Ok(new UserNameAndEmailDto()
            {
                Name = user.DisplayName,
                Email = user.Email
            });
        }

        [HttpPost("ResetPasword")]
        public async Task<ActionResult<UserDtos>> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _auth.ResetPasswordAsync(model.Email, model.NewPassword, model.Otp);

            if(user == null)
            {
                return BadRequest(new ApiResponse(400 , "حدث خطأ"));
            }

            return Ok(new UserDtos()
            {
                Name = user.DisplayName,
                Email = user.Email, 
                Token = await _auth.CreateTokenAsync(user)
            });
        }
        
    }
}
