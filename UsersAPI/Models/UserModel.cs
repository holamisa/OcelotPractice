using System.ComponentModel.DataAnnotations;
using UsersAPI.Attributes;

namespace UsersAPI.Models
{
    /// <summary>
    /// 사용자 정보 모델
    /// </summary>
    public class UserModel : BaseModel
    {
        [NotNullOrEmpty]
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }

    /// <summary>
    /// 사용자 정보 업데이트 모델
    /// </summary>
    public class UpdateUserModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(8)]
        public string? Password { get; set; }
    }
}
