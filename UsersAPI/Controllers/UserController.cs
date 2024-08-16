using Microsoft.AspNetCore.Mvc;
using UsersAPI.Models;
using UsersAPI.Services;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel user)
        {
            var result = await _userService.AddUser(user);
            return Ok(result);
        }
    }
}
