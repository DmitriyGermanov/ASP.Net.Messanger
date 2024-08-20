using UserService.DTOs;
using UserService.Models;

namespace UserService.Repo
{
    public interface IUserRepository
    {
        public Guid UserAdd(string email, string password, RoleId roleID);
        public (Guid userId, RoleId roleId) UserCheck(string email, string password);
        public Role GetRoleById(RoleId roleId);
        public IEnumerable<UserModel> GetUsers();
        public void DeleteUserById(Guid userId);
        public Guid GetUserIdFromToken();
    }
}