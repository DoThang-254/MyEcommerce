using Microsoft.AspNetCore.Mvc;
using Shared.API.Controllers;
using UserService.Application.Features.Users.Commands.Login;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Endpoint xử lý đăng nhập người dùng
        /// </summary>
        /// <param name="command">Thông tin Username và Password</param>
        /// <returns>Token và thông tin User cơ bản</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            // Mediator sẽ tìm đến LoginHandler để xử lý logic
            var result = await Mediator.Send(command);

            // Xử lý kết quả trả về từ Result<T>
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}