using LoginRegisterApi.Models;
using LoginRegisterApi.Models.Dto;
using LoginRegisterApi_Entities.Models;
using LoginRegisterApi_Entities.Models.Dto;
using LoginRegisterApi_Repository.Repositories.IRepositories;
using LoginRegisterApi_Services.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginRegisterApi_Services.Services
{
    public class UserService : IUserService
    {
        
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private string secretKey;
        protected APIResponse _response;
        public readonly IResponseService _responseService;

        public UserService(IUserRepository userRepository, ILoginRepository loginRepository, IConfiguration configuration, IResponseService responseService)
        {
            _userRepository = userRepository;
            _loginRepository = loginRepository;
            secretKey = configuration.GetValue<string>("ApiSecurity:Secret");
            this._response = new APIResponse();
            _responseService = responseService;
        }

        public async Task<APIResponse> CreateUser(UserDetailDTO userDTO)
        {
            try
            {
                LoginDetail details = await _loginRepository.GetUserLoginByEmail(userDTO.Email);
                if (details != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Email already exists." };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    //throw new BadHttpRequestException("Email already Exists");
                    return _response;
                }
                details = new();
                details.Email = userDTO.Email;
                details.Password = userDTO.Password;
                await _loginRepository.CreateUserLogin(details);

                UserDetail user = new UserDetail();
                user.Email = userDTO.Email;
                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.Age = userDTO.Age;
                user.LoginId = details.Id;
                await _userRepository.CreateUser(user);


                _response.Result = userDTO;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return _response;
            }
        }

        public async Task<APIResponse> CheckAuthUser(string email, string password)
        {
            try
            {
                LoginDetail loginDetail = await _loginRepository.GetDetails(email, password) ?? throw new BadHttpRequestException("Invalid Email or password.");
                UserDetail user = await _userRepository.GetUser(email);
                if (user == null) { return null; }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                    Issuer = user.Email
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var Token = tokenHandler.WriteToken(token);
                LoginDTO loginDTO = new()
                {
                    user = user,
                    token = Token
                };
                return _responseService.GetResponse(loginDTO, null, true, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return _responseService.GetResponse(null, new List<string> { ex.Message }, false, System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<APIResponse> GetAllUsers()
        {
            try
            {
                List<UserDetail> users = await _userRepository.GetAllUsers();
                List<UserDetailDTO> userDTOs = new List<UserDetailDTO>();
                foreach (var userInfo in users)
                {
                    UserDetailDTO user = new UserDetailDTO();
                    user.Id = userInfo.Id;
                    user.FirstName = userInfo.FirstName;
                    user.LastName = userInfo.LastName;
                    user.Email = userInfo.Email;
                    user.Age = userInfo.Age;
                    userDTOs.Add(user);
                }
                return _responseService.GetResponse(userDTOs, null, true, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return _responseService.GetResponse(null, new List<string> { ex.Message }, false, System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<APIResponse> DeleteUser(int Id)
        {
            try
            {
                UserDetail user = await _userRepository.GetUserById(Id);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid Request or User not Found" };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                await _userRepository.RemoveUser(user);

                await _loginRepository.RemoveUserLogin(user.LoginDetail);
                //return _responseService.GetResponse(null, null, true, System.Net.HttpStatusCode.OK);
                return await GetAllUsers();
            }
            catch (Exception ex)
            {
                return _responseService.GetResponse(null, new List<string> { ex.Message }, false, System.Net.HttpStatusCode.BadRequest);
            }

        }

        public async Task<APIResponse> GetUserByEmail(string email)
        {
            try
            {
                UserDetail user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid Request" };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                else
                {
                    List<UserDetailDTO> userDTOs = new List<UserDetailDTO>();
                    UserDetailDTO newUserDTO = new UserDetailDTO();
                    newUserDTO.Id = user.Id;
                    newUserDTO.FirstName = user.FirstName;
                    newUserDTO.LastName = user.LastName;
                    newUserDTO.Email = user.Email;
                    newUserDTO.Age = user.Age;
                    userDTOs.Add(newUserDTO);
                    return _responseService.GetResponse(userDTOs, null, true, System.Net.HttpStatusCode.OK);
                }
            }catch (Exception ex)
            {
                return _responseService.GetResponse(null, new List<string> { ex.Message }, false, System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<APIResponse> UpdateUser(int Id, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                UserDetail user = await _userRepository.GetUserById(Id);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid Request" };
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                user.Id = userUpdateDTO.Id;
                user.FirstName = userUpdateDTO.FirstName;
                user.LastName = userUpdateDTO.LastName;
                user.Email = userUpdateDTO.Email;
                user.Age = userUpdateDTO.Age;
                await _userRepository.UpdateUser(user);

                LoginDetail loginDetail = user.LoginDetail;
                loginDetail.Email = userUpdateDTO.Email;
                await _loginRepository.UpdateUserLogin(loginDetail);

                return await GetAllUsers();
                //return await GetUserByEmail(userUpdateDTO.Email);
            }
            catch (Exception ex)
            {
                return _responseService.GetResponse(null, new List<string> { ex.Message }, false, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
