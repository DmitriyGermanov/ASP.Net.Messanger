using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.AuthorizationModel;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController(IConfiguration configuration, IUserAuthenticationService userAuthenticationService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;

        [AllowAnonymous]
        [HttpPost("users/login")]
        public ActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var user = _userAuthenticationService.Authenticate(userLoginModel);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            else
            {
                return NotFound("User not found.");
            }
        }
        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                                           ?? throw new NullReferenceException("Key can't be Null")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}



