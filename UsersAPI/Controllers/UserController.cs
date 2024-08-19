using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Middleware.Exceptions.types;
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
        public async Task<IActionResult> GetById([FromQuery] int id)
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateById([FromQuery] int id, [FromBody] JsonPatchDocument<UpdateUserModel> patchDoc)
        {
            var user = new UpdateUserModel();
            patchDoc.ApplyTo(user, ModelState);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException("There were validation errors", errors);
            }

            var result = await _userService.GetUserById(id);
            return Ok(result);
        }
    }
}
