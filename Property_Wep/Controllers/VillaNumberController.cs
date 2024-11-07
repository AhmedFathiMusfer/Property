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
       

        public VillaNumberController(IMapper mapper,IVillaNumberService villaNumberService, IVillaService villaService)
        {
            this.mapper = mapper;
            this.villaNumberService = villaNumberService;
            this.villaService = villaService;
       
        }

        public async Task< IActionResult>IndexVillaNumber()
        {
           List<VillaNumberDTO> villaNumberDTO = new();
            var Response =await villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
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
                var Response = await villaNumberService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (Response != null && Response.IsSuccess )
                {
                    TempData["Success"] = "Villa Number Created Successfuly";
                    return Redirect(nameof(IndexVillaNumber));   
                   
                }
                else
                {
                    if (Response.ErrorMessages != null) 
                    {
                        int i = 0;
                        foreach (var Error in Response.ErrorMessages)
                        {
                            i++;
                            ModelState.AddModelError($"ErrorMessage {i}", Error);
                        }


                    }
                   
                }
            }

            ViewBag.Villas = await GetVilla();

            TempData["error"] = "Error encountered";
            return View(model);
        }

        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            var respose =  await villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (respose != null && respose.IsSuccess)
            {
                VillaNumberDTO villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(respose.Result));

                ViewBag.Villas = await GetVilla();
                return View(mapper.Map<VillaNumberUpdateDTO>(villaNumber));
            }


            return NotFound();

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
        {
           
            if (ModelState.IsValid)
            {
                var Response = await villaNumberService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (Response != null && Response.IsSuccess )
                {
                    TempData["Success"] = "Villa Number Updated Successfuly";
                    return Redirect(nameof(IndexVillaNumber));

                }
            }

            ViewBag.Villas = await GetVilla();
            TempData["error"] = "Error encountered";

            return View(model);
        }

        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            var respose = await villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (respose != null && respose.IsSuccess)
            {
                VillaNumberDTO villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(respose.Result));

              
                return View(villaNumber);
            }


            return NotFound();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO model)
        {

            var Response = await villaNumberService.RemoveAsync<APIResponse>(model.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (Response != null && Response.IsSuccess)
            {
                TempData["Success"] = "Villa Number Deleted Successfuly";
                return Redirect(nameof(IndexVillaNumber));

            }


            TempData["error"] = "Error encountered";

            return View(model);
        }





        private async Task<IEnumerable<SelectListItem>> GetVilla()
        {
            List<VillaDTO> villaDTOs = new();
            var Response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
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
