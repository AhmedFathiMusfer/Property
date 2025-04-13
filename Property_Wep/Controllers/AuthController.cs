using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;
using Property_WepAPI.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Property_Wep.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHttpContextAccessor _contextAccessor; 

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() {

             new SelectListItem()
             {
                 Text = "Admin",
                 Value="admin"
             },
             new SelectListItem()
             {
                 Text="Customer",
                 Value = "customer"
             }
            
            };
            ViewBag.RoleList = roleList;
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var response = await _authService.RegisterAsync<APIResponse>(registerationRequestDTO);
            if (response.IsSuccess && response != null)
            {
                return RedirectToAction("Login");
            }
            var Error = response.ErrorMessages.FirstOrDefault();
            ModelState.AddModelError("ErrorMessage", Error);
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
   
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var response = await _authService.LoginAsync<APIResponse>(loginRequestDTO);
            if (response.IsSuccess && response != null)
            {
                TokenDTO model = JsonConvert.DeserializeObject<TokenDTO>(Convert.ToString(response.Result));

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(model.AccessToken);


                identity.AddClaim(new Claim(ClaimTypes.Name, token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, token.Claims.FirstOrDefault(u=>u.Type=="role").Value));
                var principal = new ClaimsPrincipal(identity);
              await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _tokenProvider.SetToken(model);
               return  RedirectToAction("Index", "Home");
            }

            var Error = response.ErrorMessages.FirstOrDefault();
            ModelState.AddModelError("ErrorMessage", Error);
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync();
            var TokenDto = _tokenProvider.GetToken();
         var Response=  await  _authService.LogoutAsync<APIResponse>(TokenDto);
            if (Response.IsSuccess)
            {
                _tokenProvider.ClearToken();
                return RedirectToAction("Login");
            }
            return View();
           
            
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {


            return View();
        }

        [HttpGet] 
        public IActionResult ForegetPasswored()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForegetPasswored(ForgetPassworedDto forgetPassworedDto)
        {
            var response = await _authService.ForgetPassworedAsync<APIResponse>(forgetPassworedDto);
            if (response != null && response.IsSuccess)
            {
                ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO = new()
                {
                    Email = forgetPassworedDto.Email
                };
                return View("ForegetPassworedConfirmation",forgetPassworedConfirmationDTO);
            }

            var Error = response.ErrorMessages.FirstOrDefault();
            ModelState.AddModelError("ErrorMessage", Error);
            return View(forgetPassworedDto);
        }
        
      
        [HttpPost]
        public async Task<IActionResult> ForegetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO)
        {
            var response = await _authService.ForgetPassworedAsyncConfirmationAsync<APIResponse>(forgetPassworedConfirmationDTO);
            if (response != null && response.IsSuccess)
            {
                ResetPassworedDTO resetPassworedDTO = new()
                {
                    Email = forgetPassworedConfirmationDTO.Email,
                    Code = forgetPassworedConfirmationDTO.Code
                };
                return View("ResetPasswored",resetPassworedDTO);
            }
            var Error = response.ErrorMessages.FirstOrDefault();
            ModelState.AddModelError("ErrorMessage", Error);
            return View(forgetPassworedConfirmationDTO);
        }
    
        [HttpPost]
        public async Task<IActionResult> ResetPasswored(ResetPassworedDTO resetPassworedDTO)
        {
            var response=await _authService.ResetPasswored<APIResponse>(resetPassworedDTO);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }
            var Error = response.ErrorMessages.FirstOrDefault();
            ModelState.AddModelError("ErrorMessage", Error);
            return View(resetPassworedDTO);
        }
    }
}
