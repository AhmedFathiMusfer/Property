using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;

namespace Property_Wep.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVillaService villaService;
        private string Session;

        public HomeController(IMapper mapper, IVillaService villaService)
        {
            this.mapper = mapper;
            this.villaService = villaService;
           // Session = HttpContext.Session.GetString(SD.SessionToken);
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaDTO = new();
            var Response = await villaService.GetAllAsync<APIResponse>();
            if (Response != null && Response.IsSuccess)
            {
                villaDTO = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(Response.Result));
            }


            return View(villaDTO);
        }
    }
}
