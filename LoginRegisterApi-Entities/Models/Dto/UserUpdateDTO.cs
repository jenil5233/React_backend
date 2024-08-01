using System.ComponentModel.DataAnnotations;

namespace LoginRegisterApi_Entities.Models.Dto
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
