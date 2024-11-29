using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Controllers;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.Models.Authentication.Login;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserManagement _user;


        // public AuthenticationController(UserManager<IdentityUser> userManager,
        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, 
            // IEmailService emailService, SignInManager<IdentityUser> signInManager, IUserManagement user)
            IEmailService emailService, SignInManager<ApplicationUser> signInManager, IUserManagement user)
        {
            _userManager = userManager;
            _user = user;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _signInManager = signInManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var tokenResponse = await _user.CreateUserWithTokenAsync(registerUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { tokenResponse.Response!.Token, email = registerUser.Email }, Request.Scheme);
                var message = new Message(new string[] { registerUser.Email! }, "Confirmation email link", confirmationLink!);
                _emailService.SendEmail(message);

                return ApiResponse.Success( $"User created & Email Sent to {registerUser.Email} SuccessFully");                  
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
                    return ApiResponse.Success( "Email Verified Successfully");
                }
            }
            return ApiResponse.BadRequest( "This User Doesnot exist!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var loginOtpResponse=await _user.GetOtpByLoginAsync(loginModel);
            if (loginOtpResponse.Response!=null)
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
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
        }

        }

        return Unauthorized();

        }  

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code,string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var signIn= await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
            if (signIn.Succeeded)
            {
                if (user != null )
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                    //returning the token...

                }
            }
            return ApiResponse.BadRequest("Invalid Code");
        }
       
       [HttpPost]
       [AllowAnonymous]
       [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email) 
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null) 
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
            var model = new ResetPassword { Token = token, Email = email};
            return Ok(new {
                model
            });
        }

        [HttpPost]
       [AllowAnonymous]
       [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword) 
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user != null) 
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if(!resetPassResult.Succeeded)
                {
                    foreach(var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return ApiResponse.Success($"Password has been changed");
            }
            return ApiResponse.BadRequest("Something wrong..Try Again");
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    
}
}