using LoginRegisterApi_Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegisterApi.Models
{
    public class UserDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string Email { get; set; }

       

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        [ForeignKey("LoginDetail")]
        public int LoginId { get; set; }

        public LoginDetail LoginDetail { get; set; }

    }
}
