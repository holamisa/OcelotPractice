using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Middleware.Exceptions.types;
using System.Net;
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
            UserModel? user = await _userService.GetUserById(id, true);
            return Ok(ApiResponseModel<UserModel>.OK(user));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidationException("There were validation errors", errors);
            }

            var result = await _userService.AddUser(user);
            return StatusCode((int)HttpStatusCode.Created, ApiResponseModel.CREATED());
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] JsonPatchDocument<UpdateUserModel> patchDoc)
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

            user.Id = id;

            await _userService.UpdateUserById(user);
            return Ok(ApiResponseModel.OK());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserById(id);
            return Ok(ApiResponseModel.OK());
        }
    }
}
