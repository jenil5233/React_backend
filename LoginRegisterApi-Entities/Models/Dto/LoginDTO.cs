using LoginRegisterApi.Models;


namespace LoginRegisterApi_Entities.Models.Dto
{
    public class LoginDTO
    {
        public UserDetail user { get; set; }

        public string token { get; set; }
    }
}
