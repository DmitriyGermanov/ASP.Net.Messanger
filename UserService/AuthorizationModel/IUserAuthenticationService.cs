using UserService.DTOs;
using UserService.Models;

namespace UserService.AuthorizationModel
{
    public interface IUserAuthenticationService
    {
        User? Authenticate(UserLoginModel model);
    }
}
