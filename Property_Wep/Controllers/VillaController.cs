using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Property_Utility;
using Property_Wep.Models;
using Property_Wep.Models.Dto;
using Property_Wep.Services.IServices;
using System.Linq.Expressions;

namespace Property_Wep.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;
       // private string Session;

        public VillaController(IVillaService villaService,IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
            
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villaDTOs = new();


            var Response= await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (Response!=null&&Response.IsSuccess)
            {
                villaDTOs =JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString( Response.Result));
            }
          //  SaveToExcel(villaDTOs, @"C:\Users\EBH\Desktop\file.xlsx");
            return View(villaDTOs);
        }

        public IActionResult CreateVilla()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (Response!=null && Response.IsSuccess)
                {
                    TempData["Success"] = "Villa Created Successfuly";
                    return Redirect(nameof(IndexVilla));
                }
             
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }

        public async Task<IActionResult>UpdateVilla(int villaId)
        {
            var Response = await villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
            if (Response != null && Response.IsSuccess)
            {
                VillaDTO villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(Response.Result));
                return View(mapper.Map<VillaUpdateDTO>(villaDTO));
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await villaService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (Response != null && Response.IsSuccess)
                {
                    TempData["Success"] = "Villa Updated Successfuly";
                    return Redirect(nameof(IndexVilla));

                }
                
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }

        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            VillaDTO villaDTO=new();
            var Response = await villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
            if (Response.Result != null && Response.IsSuccess)
            {
                villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(Response.Result));
                
                return View(villaDTO);
            }
           
            return NotFound();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
           
            var Response = await villaService.RemoveAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (Response!=null&& Response.IsSuccess)
            {
                TempData["Success"] = "Villa Deleted Successfuly";

                return Redirect(nameof(IndexVilla));
            }
            TempData["error"] = "Error encountered";
            return View(model);

        }

        //public void SaveToExcel(List<VillaDTO> products, string filePath)
        //{
        //    // تحقق من تفعيل الترخيص لمكتبة EPPlus 
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        // إنشاء ورقة عمل جديدة في الملف
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

        //        // إضافة رؤوس الأعمدة
        //        worksheet.Cells[1, 1].Value = "Name";
        //        worksheet.Cells[1, 2].Value = "Price";
        //        worksheet.Cells[1, 3].Value = "Quantity";

        //        // إضافة البيانات
        //        int row = 2;
        //        foreach (var product in products)
        //        {
        //            worksheet.Cells[row, 1].Value = product.Name;
        //            worksheet.Cells[row, 2].Value = product.Occupancy;
        //            worksheet.Cells[row, 3].Value = product.Rate;
        //            row++;
        //        }

        //        // حفظ الملف
        //        FileInfo file = new FileInfo(filePath);
        //        package.SaveAs(file);
        //    }
        //}
    }
}
