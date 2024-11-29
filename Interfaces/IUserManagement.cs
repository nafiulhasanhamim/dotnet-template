using api.Controllers;
using dotnet_mvc.Models.Authentication.Login;
using dotnet_mvc.Models.Authentication.User;
using User.Management.Service.Models;
using User.Management.Service.Models.Authentication.SignUp;

namespace dotnet_mvc.Interfaces
{
    public interface IUserManagement
    {
        Task<ApiResponseUser<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUser registerUser);
        Task<ApiResponseUser<LoginOtpResponse>> GetOtpByLoginAsync(LoginModel loginModel);
        
    }
}
