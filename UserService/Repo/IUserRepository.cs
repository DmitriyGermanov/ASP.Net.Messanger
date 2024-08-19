using UserService.DTOs;
using UserService.Models;

namespace UserService.Repo
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string password, RoleId roleID);
        public RoleId UserCheck(string name, string password);
        public Role GetRoleById(RoleId roleId);
        public IEnumerable<UserModel> GetUsers();
        public void DeleteUserByEmail(string email);
        public string GetEmailFromToken();
    }
}
