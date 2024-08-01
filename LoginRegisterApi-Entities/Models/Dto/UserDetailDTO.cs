using System.ComponentModel.DataAnnotations;

namespace LoginRegisterApi.Models.Dto
{
    public class UserDetailDTO
    {
        [Required]
        public string Email { get; set; }

        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int Age { get; set; }

    }
}
