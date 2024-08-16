using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class CartModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
