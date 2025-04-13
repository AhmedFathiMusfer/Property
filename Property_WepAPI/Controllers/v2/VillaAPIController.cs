using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Property_WepAPI.Data;
using Property_WepAPI.logging;
using Property_WepAPI.MapppingConfig;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repositry.IRpositry;
using System.Net;

namespace Property_WepAPI.Controllers.v2
{
    [Route("api/v{version:ApiVersion}/VillaAPI")]
    [ApiController]
   
    [ApiVersion("2.0")]
    [Authorize]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapping;
        private readonly ILogging _logger;

        protected APIResponse _response;


        public VillaAPIController(ILogging logger, IVillaRepository dbVilla, IMapper mapping)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapping = mapping;
            _response = new();
        }




        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetVills([FromQuery(Name = "filterOccupancy")] int? occupancy,
            [FromQuery(Name = "Search")] string? Search, int pagesize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Villa> villas;
                if (occupancy > 0)
                {
                    villas = await _dbVilla.GetAllAsync(v => v.Occupancy == occupancy, pageSize: pagesize, pageNumber: pageNumber);
                }
                else
                {
                    villas = await _dbVilla.GetAllAsync(pageSize: pagesize, pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    villas = villas.Where(v => v.Name.ToLower().Contains(Search.ToLower()));
                }
                var pagination = new Pagination { pageSize = pagesize, pageNumber = pageNumber };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
                _response.Result = _mapping.Map<List<VillaDTO>>(villas);
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetVill(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error The ID = 0");

                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Is not Found");
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = _mapping.Map<VillaDTO>(villa);
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;



        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Add([FromForm] VillaCreateDTO villaDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("villa already exist");
                    return BadRequest(_response);
                }
                if (villaDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }

                Villa model = _mapping.Map<Villa>(villaDTO);
                await _dbVilla.CreateAsync(model);
                if(villaDTO.Image != null)
                {
                    var fileName = model.Id.ToString() + Path.GetExtension(villaDTO.Image.FileName);
                    var filePath = @"wwwroot\VillaImages\" + fileName;
                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    FileInfo file = new FileInfo(directoryLocation);
                    if(file.Exists)
                    {
                        file.Delete();
                    }
                    using(var fileStream=new FileStream(directoryLocation,FileMode.Create))
                    {
                        villaDTO.Image.CopyTo(fileStream);
                    }
                    var basePath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    model.ImageUrl = basePath+ "/VillaImages/" + fileName;
                    model.LocalImagePath = filePath;
                    
                }
                else
                {
                    model.ImageUrl = "https://placehold/600x400";
                }
                await _dbVilla.UpdateAsync(model); 
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapping.Map<VillaDTO>(model); ;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;



        }
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error The ID = 0");
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Is not Found");
                    return NotFound(_response);
                }
                if (!string.IsNullOrEmpty(villa.LocalImagePath))
                {

                    var OldDirectoryLocation = Path.Combine(Directory.GetCurrentDirectory(), villa.LocalImagePath);

                    FileInfo file = new FileInfo(OldDirectoryLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                }

                await _dbVilla.RemoveAsync(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;



        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromForm] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (id != villaDTO.Id || villaDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {

                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Is not Found");
                    return NotFound(_response);
                }
                Villa model = _mapping.Map<Villa>(villaDTO);
                if(villaDTO.Image!=null)
                {
                    if(!string.IsNullOrEmpty(model.LocalImagePath)) { 
                       
                        var OldDirectoryLocation=Path.Combine(Directory.GetCurrentDirectory (), model.LocalImagePath);

                        FileInfo file=new FileInfo (OldDirectoryLocation);
                        if(file.Exists)
                        {
                            file.Delete();
                        }
                    
                    }
                    var fileName=villaDTO.Id.ToString()+Path.GetExtension(villaDTO.Image.FileName);
                    var filePath = @"wwwroot\VillaImages\" + fileName;
                    var DirectoryLocation=Path.Combine (Directory.GetCurrentDirectory (), filePath);
                    using (var fileStream = new FileStream(DirectoryLocation, FileMode.Create))
                    {

                        villaDTO.Image.CopyTo(fileStream);
                    }
                    var basePath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    model.ImageUrl = basePath + "/VillaImages/" + fileName; 
                    model.LocalImagePath = filePath;
                    
                   
                }
                await _dbVilla.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;




        }

        [HttpPatch("{id:int}", Name = "UpdatePartiolVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartiolVilla(int id, JsonPatchDocument<VillaUpdateDTO> partiolDTO)
        {
            try
            {
                if (id == 0 || partiolDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response); ;
                }
                Villa villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {

                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Is not Found");
                    return NotFound(_response);
                }
                VillaUpdateDTO villaDTO = _mapping.Map<VillaUpdateDTO>(villa);

                partiolDTO.ApplyTo(villaDTO, ModelState);
                if (!ModelState.IsValid)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response); ;
                }
                Villa modle = _mapping.Map<Villa>(villaDTO);
                await _dbVilla.UpdateAsync(modle);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _response;


        }

    }
}
