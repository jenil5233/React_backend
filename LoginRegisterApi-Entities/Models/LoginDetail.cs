using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace LoginRegisterApi_Entities.Models
{
    public class LoginDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
