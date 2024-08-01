using LoginRegisterApi_Entities.Models;

namespace LoginRegisterApi_Repository.Repositories.IRepositories
{
    public interface ILoginRepository
    {
        Task<LoginDetail> GetDetails(string email, string password);

        Task<LoginDetail> GetUserLoginByEmail(string email);

        Task<LoginDetail> CreateUserLogin(LoginDetail loginDetail);

        Task<bool> RemoveUserLogin(LoginDetail loginDetail);

        Task<LoginDetail> UpdateUserLogin(LoginDetail loginDetail);
    }
}
