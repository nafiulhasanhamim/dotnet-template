using api.Controllers;
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.Models.Authentication.Login;
using dotnet_mvc.Models.Authentication.User;
using Microsoft.AspNetCore.Identity;
using User.Management.Service.Models;
using User.Management.Service.Models.Authentication.SignUp;

public class UserManagementService : IUserManagement
{
    // private readonly UserManager<IdentityUser> _userManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    // private readonly SignInManager<IdentityUser> _signInManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // public UserManagementService(UserManager<IdentityUser> userManager,
    public UserManagementService(UserManager<ApplicationUser> userManager,
        // RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<ApiResponseUser<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUser registerUser)
    {
        // Check if the user already exists
        //Check User Exist 
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email!);
            var userNameExist = await _userManager.FindByNameAsync(registerUser.Username!);

            if (userExist != null)
            {
                return new ApiResponseUser<CreateUserResponse> { IsSuccess = false, StatusCode = 403, Message = "User already exists!" };
            }
            else if(userNameExist != null) 
            {
                return new ApiResponseUser<CreateUserResponse> { IsSuccess = false, StatusCode = 403, Message = "User with this userName already exists!" };
            }
            //Add the User in the database
            // IdentityUser user = new()
            ApplicationUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                TwoFactorEnabled=true
            };
            // var roleCheck = await _roleManager.RoleExistsAsync(registerUser.Role!);

            if (await _roleManager.RoleExistsAsync(registerUser.Role!))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password!);
                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("Password"))
                    {
                        var message = $"Weak password: {error.Description}";
                        return new ApiResponseUser<CreateUserResponse> { IsSuccess = false, StatusCode = 403, Message = message };
                    }
                }
                if (!result.Succeeded)
                {
                    return new ApiResponseUser<CreateUserResponse> { IsSuccess = false, StatusCode = 403, Message = "User failed to create" };
                }
                //Add role to the user....
                await _userManager.AddToRoleAsync(user, registerUser.Role!);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return new ApiResponseUser<CreateUserResponse> { IsSuccess = true, StatusCode = 201, Message = "User is created", Response = new CreateUserResponse() { Token= token  } };
    } 
    else
    {
        return new ApiResponseUser<CreateUserResponse> { IsSuccess = false, StatusCode = 403, Message = "This role doesnot exists" }; 
    }

}

    public async Task<ApiResponseUser<LoginOtpResponse>> GetOtpByLoginAsync(LoginModel loginModel)
        {   
            var user = await _userManager.FindByNameAsync(loginModel.Username!);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginModel.Password!, false, true);
                if (user.TwoFactorEnabled)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    return new ApiResponseUser<LoginOtpResponse>
                    {
                        Response = new LoginOtpResponse()
                        {
                            User = user,
                            Token = token,
                            IsTwoFactorEnable = user.TwoFactorEnabled
                        },
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = $"OTP send to the email {user.Email}"
                    };

                }
                else
                {
                    return new ApiResponseUser<LoginOtpResponse>
                    {
                        Response = new LoginOtpResponse()
                        {
                            Token = string.Empty,
                            IsTwoFactorEnable = user.TwoFactorEnabled
                        },
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = $"2FA is not enabled"
                    };
                }
            }
            else
            {
                return new ApiResponseUser<LoginOtpResponse>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = $"User doesnot exist."
                };
            }
        }
}