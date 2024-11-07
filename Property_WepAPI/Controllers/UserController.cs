using Microsoft.AspNetCore.Mvc;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repository.IRpository;

namespace Property_WepAPI.Controllers
{

    [Route("api/v{version:ApiVersion}/AuthUser")]
    [ApiController]
    [ApiVersionNeutral]

    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        public APIResponse _response;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginReponse = await userRepository.Login(loginRequestDTO);
            if (loginReponse.User == null || string.IsNullOrEmpty(loginReponse.Token))
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User Name or PassWord is Incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginReponse;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            bool IsUnique = userRepository.IsUniqueUser(registerationRequestDTO.UserName);
            if (!IsUnique)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User is already Exist");
                return BadRequest(_response);
            }

            var user = await userRepository.Register(registerationRequestDTO);
            if (user == null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while register");
                return BadRequest(_response);

            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }
    }
}
