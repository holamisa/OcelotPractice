using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class UserModel : BaseModel
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }

    public class UpdateUserModel
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(8)]
        public string? Password { get; set; }
    }
}
