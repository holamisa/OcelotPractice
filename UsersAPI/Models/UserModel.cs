using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class UserModel : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }
}
