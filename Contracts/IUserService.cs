using System.Threading.Tasks;
using Entities;

namespace Contracts;

public interface IUserService
{
   
        public Task<User> GetUserByUsername(string username);
        public Task<User> GetUserById(long id);
        public Task<string?> GetUserTokenById(long id);
        public Task<User> GetUserByEmail(string email);
        public Task<User> RegisterUser(User user);
        public Task<User> RemoveUser(long userId);
        public Task<User> UpdateUser(User user);
        public Task<User> LogUserIn(string email, string password);
        public  Task SetTokenForUser(long uId, string token);
        public  Task RemoveTokenFromUser(long uId);
}
