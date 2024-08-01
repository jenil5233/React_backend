using LoginRegisterApi.Models.Dto;
using LoginRegisterApi_Entities.Models;
using LoginRegisterApi_Entities.Models.Dto;
using LoginRegisterApi_Services.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterApi.Controllers
{
    [ApiController]
    [Route("api/HomeAPI")]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;
        protected APIResponse _response;
        
        public HomeController(IUserService userService)
        {
            _userService = userService;
            this._response = new APIResponse();
            
        }

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserByEmail([FromBody]string email)
        {
            if(email == null)
            {
                throw new BadHttpRequestException("Bad Request");
            }
            var response = await _userService.GetUserByEmail(email);
            return Ok(response);
        }

        //[Route("/HomeAPI/CheckUser")]
        [HttpPost("CheckUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckUser([FromBody]Credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest();
            }
            var user = await _userService.CheckAuthUser(credentials.Email, credentials.Password);
            return Ok(user);
        }

        //[Route("/HomeAPI/CreateUser")]
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDetailDTO>> CreateUser([FromBody]UserDetailDTO user)
        {
            if(user == null)
            {
                throw new BadHttpRequestException("Invalid Input Values", 400);
            }
            var newuser = await _userService.CreateUser(user);
            return Ok(newuser);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var result = await _userService.DeleteUser(Id);

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser(int Id, [FromBody]UserUpdateDTO userUpdateDTO)
        {
            if (Id == 0 | Id != userUpdateDTO.Id | userUpdateDTO == null)
            {
                return BadRequest();
            }
            var result = await _userService.UpdateUser(Id, userUpdateDTO);
            return Ok(result);
        }

        
        //[Route("/HomeAPI/UpdateUser")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPut]
        //public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        //{
        //    UserDetail? user = await _userService.UpdateUserDetail(userUpdateDTO);
        //    if (user == null)
        //    {
        //        return NotFound(new {message = "user not found"});
        //    }

        //    return Ok(new {message = "Details updated successfully" , data = user});
        //}
    }
}
