using LoginRegisterApi.Models.Dto;
using LoginRegisterApi_Entities.Models;
using LoginRegisterApi_Entities.Models.Dto;

namespace LoginRegisterApi_Services.Services.IServices
{
    public interface IUserService
    {
        Task<APIResponse> CreateUser(UserDetailDTO userDTO);

        Task<APIResponse> CheckAuthUser(string email, string password);

        Task<APIResponse> GetAllUsers();

        Task<APIResponse> DeleteUser(int Id);

        Task<APIResponse> UpdateUser(int Id, UserUpdateDTO userUpdateDTO);
        Task<APIResponse> GetUserByEmail(string email);
    }
}
