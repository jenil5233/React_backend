using LoginRegisterApi.Models;

namespace LoginRegisterApi_Repository.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<UserDetail> CreateUser(UserDetail user);

        Task<UserDetail> GetUser(string email);

        Task<UserDetail> GetUserById(int id);

        Task<List<UserDetail>> GetAllUsers();

        Task<bool> RemoveUser(UserDetail user);

        Task<UserDetail> UpdateUser(UserDetail user);

        Task<UserDetail> GetUserByEmail(string email);
    }
}
