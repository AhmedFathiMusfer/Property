using Microsoft.AspNetCore.Mvc;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repository.IRpository;
using System.Net;

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
            if (loginReponse == null || string.IsNullOrEmpty(loginReponse.AccessToken))
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
        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshTokrn([FromBody]TokenDTO  tokenDTO)
        {
            if(ModelState.IsValid)
            {
                var refreshToken = await userRepository.RefreshAccessToken(tokenDTO);
                if (refreshToken == null || string.IsNullOrEmpty(refreshToken.AccessToken))
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Refresh Token is invalid");
                    return BadRequest(_response);
                }
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = refreshToken;
                return Ok(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Invilad Input");
            return BadRequest(_response);


        }
        [HttpPost("revoke")]
        
        public async Task<IActionResult> RevokeRefreshToken([FromBody]TokenDTO tokenDTO)
        {
            if(ModelState.IsValid) {
                try
                {
                    await userRepository.RevokeAccessToken(tokenDTO);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                }
              
            }
            _response.IsSuccess = false ;
            _response.ErrorMessages.Add("Invalid Input");
            return BadRequest(_response);
        }

        [HttpPost("forgetPasswored")]
        public async Task<IActionResult> ForgetPasswored(ForgetPassworedDTO forgetPassworedDTO)
        {
            if(ModelState.IsValid)
            {
                var forgetPasswored = await userRepository.ForgetPasswored(forgetPassworedDTO);
                if(forgetPasswored != null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.OK ;
                    _response.IsSuccess = true;
                    _response.Result=forgetPasswored ;
                    return Ok(_response);
                }
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Email Is Invalid");
            return BadRequest(_response);
        }
        [HttpPost("forgetPassworedConfirmation")]
        public async Task<IActionResult> ForgetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO)
        {
            if(ModelState.IsValid)
            {
                var forgetPassworedConfirmation = await userRepository.ForgetPassworedConfirmation(forgetPassworedConfirmationDTO); 
                if(forgetPassworedConfirmation != null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = forgetPassworedConfirmation ;
                    return Ok(_response);
                }
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("code Is Invalid");
            return BadRequest(_response);
        }
        [HttpPost("resetPasswored")]
        public async Task<IActionResult> ResetPasswored(ResetPassworedDTO resetPassworedDTO)
        {
            if (ModelState.IsValid)
            {
                var resetPasswored = await userRepository.ResetPasswored(resetPassworedDTO);
                if (resetPasswored )
                {
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
            }
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Reset Passwored Is Invalid");
            return BadRequest(_response);
        }
    }
}
