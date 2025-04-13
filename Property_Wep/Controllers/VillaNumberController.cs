using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services;
using Property_Wep.Services.IServices;
using System.Net;

namespace Property_Wep.Controllers
{
    [Authorize]
    public class VillaNumberController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVillaNumberService villaNumberService;
        private readonly IVillaService villaService;
        private readonly ITokenProvider _tokenProvider;


        public VillaNumberController(IMapper mapper,IVillaNumberService villaNumberService, IVillaService villaService, ITokenProvider tokenProvider)
        {
            this.mapper = mapper;
            this.villaNumberService = villaNumberService;
            this.villaService = villaService;
            _tokenProvider = tokenProvider;
        }

        public async Task< IActionResult>IndexVillaNumber()
        {
           List<VillaNumberDTO> villaNumberDTO = new();
            var Response =await villaNumberService.GetAllAsync<APIResponse>();
            if (Response != null && Response.IsSuccess)
            {
                villaNumberDTO = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(Response.Result));
            }
            

            return View(villaNumberDTO);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {

            ViewBag.Villas = await GetVilla();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
        {
            List<VillaDTO> villaDTOs = new();
            if (ModelState.IsValid)
            {
                var Response = await villaNumberService.CreateAsync<APIResponse>(model);
                if (Response != null && Response.IsSuccess )
                {
                    TempData["Success"] = "Villa Number Created Successfuly";
                    return Redirect(nameof(IndexVillaNumber));   
                   
                }
                else
                {
                    TempData["error"] =( Response.ErrorMessages != null && Response.ErrorMessages.Count > 0) ? Response.ErrorMessages[0]:"Error Encounetred" ;

                }
            }

            ViewBag.Villas = await GetVilla();

          
            return View(model);
        }

        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            var Response =  await villaNumberService.GetAsync<APIResponse>(villaNo );
            if (Response != null && Response.IsSuccess)
            {
                VillaNumberDTO villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(Response.Result));

                ViewBag.Villas = await GetVilla();
                return View(mapper.Map<VillaNumberUpdateDTO>(villaNumber));
            }
            else
            {
                TempData["error"] = (Response.ErrorMessages != null && Response.ErrorMessages.Count > 0) ? Response.ErrorMessages[0] : "Error Encounetred";

            }

            return NotFound();

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
        {
           
            if (ModelState.IsValid)
            {
                var Response = await villaNumberService.UpdateAsync<APIResponse>(model );
                if (Response != null && Response.IsSuccess )
                {
                    TempData["Success"] = "Villa Number Updated Successfuly";
                    return Redirect(nameof(IndexVillaNumber));

                }
                else
                {
                    TempData["error"] = (Response.ErrorMessages != null && Response.ErrorMessages.Count > 0) ? Response.ErrorMessages[0] : "Error Encounetred";

                }
            }


            ViewBag.Villas = await GetVilla();
           

            return View(model);
        }

        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            var Response = await villaNumberService.GetAsync<APIResponse>(villaNo);
            if (Response != null && Response.IsSuccess)
            {
                VillaNumberDTO villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(Response.Result));

              
                return View(villaNumber);
            }
            else
            {
                TempData["error"] = (Response.ErrorMessages != null && Response.ErrorMessages.Count > 0) ? Response.ErrorMessages[0] : "Error Encounetred";

            }


            return NotFound();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO model)
        {

            var Response = await villaNumberService.RemoveAsync<APIResponse>(model.VillaNo );
            if (Response != null && Response.IsSuccess)
            {
                TempData["Success"] = "Villa Number Deleted Successfuly";
                return Redirect(nameof(IndexVillaNumber));

            }
            else
            {
                TempData["error"] = (Response.ErrorMessages != null && Response.ErrorMessages.Count > 0) ? Response.ErrorMessages[0] : "Error Encounetred";

            }


           

            return View(model);
        }





        private async Task<IEnumerable<SelectListItem>> GetVilla()
        {
            List<VillaDTO> villaDTOs = new();
            var Response = await villaService.GetAllAsync<APIResponse>();
            if (Response.IsSuccess)
            {
                villaDTOs = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(Response.Result));
            }
            var selectListItems = villaDTOs.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
           return  selectListItems;
        }

    }
}
