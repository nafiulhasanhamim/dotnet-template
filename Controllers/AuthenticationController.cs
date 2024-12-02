using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Controllers;
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.Models.Authentication.Login;
using dotnet_mvc.Models.Authentication.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using User.Management.Service.Models.Authentication.SignUp;

namespace dotnet_mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // private readonly UserManager<IdentityUser> _userManager;
        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly SignInManager<IdentityUser> _signInManager;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        // private readonly RoleManager<IdentityRole> _roleManager;
        // private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserManagement _user;


        // public AuthenticationController(UserManager<IdentityUser> userManager,
        public AuthenticationController(UserManager<ApplicationUser> userManager,
            // RoleManager<IdentityRole> roleManager,
            // IEmailService emailService, SignInManager<IdentityUser> signInManager, IUserManagement user)
            // SignInManager<ApplicationUser> signInManager
            IEmailService emailService, IUserManagement user
            )
        {
            _userManager = userManager;
            _user = user;
            // _roleManager = roleManager;
            _emailService = emailService;
            // _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var tokenResponse = await _user.CreateUserWithTokenAsync(registerUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { tokenResponse.Response!.Token, email = registerUser.Email }, Request.Scheme);
            var message = new Message(new string[] { registerUser.Email! }, "Confirmation email link", confirmationLink!);
            _emailService.SendEmail(message);

            return ApiResponse.Success($"User created & Email Sent to {registerUser.Email} SuccessFully");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return ApiResponse.Success("Email Verified Successfully");
                }
            }
            return ApiResponse.BadRequest("This User Doesnot exist!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var loginOtpResponse = await _user.GetOtpByLoginAsync(loginModel);
            if (loginOtpResponse.Response != null)
            {
                var user = loginOtpResponse.Response.User;
                if (user.TwoFactorEnabled)
                {
                    var token = loginOtpResponse.Response.Token;
                    var message = new Message(new string[] { user.Email! }, "OTP Confrimation", token);
                    _emailService.SendEmail(message);

                    return ApiResponse.Success($"We have sent an OTP to your Email {user.Email}");
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password!))
                {
                    var serviceResponse = await _user.GetJwtTokenAsync(user);
                    return ApiResponse.Success(serviceResponse);
                }

            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(LoginWithOTPDTO loginWithOTP)
        {
            var jwt = await _user.LoginUserWithJWTokenAsync(loginWithOTP.Code, loginWithOTP.Username);
            if (jwt.IsSuccess)
            {
                return ApiResponse.Success(jwt);

            }
            return ApiResponse.BadRequest("Invalid code");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Forgot password link", forgotPasswordLink!);
                _emailService.SendEmail(message);
                return ApiResponse.Success($"Password changed request is sent on email: ${user.Email}");
            }
            return ApiResponse.BadRequest("Something wrong..Try Again");
        }

        [HttpGet("reset-password")]

        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new
            {
                model
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return ApiResponse.Success($"Password has been changed");
            }
            return ApiResponse.BadRequest("Something wrong..Try Again");
        }

        [HttpPost]
        [Route("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(LoginResponse tokens)
        {
            var jwt = await _user.RenewAccessTokenAsync(tokens);
            if (jwt.IsSuccess)
            {
                return ApiResponse.Success(jwt);
            }
            return ApiResponse.BadRequest("Invalid Code");
        }

    }
}