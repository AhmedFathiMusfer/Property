using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Property_WepAPI.Data;
using Property_WepAPI.logging;
using Property_WepAPI.MapppingConfig;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using Property_WepAPI.Repositry.IRpositry;
using System.Net;

namespace Property_WepAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbvillaNumber;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapping;
        private readonly ILogging _logger;

        protected  APIResponse _APIResponse;
       

       public  VillaNumberAPIController(ILogging logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapping)
        {
            _logger = logger;
            _dbvillaNumber = dbVillaNumber;
            _mapping = mapping;
            _dbVilla = dbVilla;
            _APIResponse=new();
        }

         [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillNumbers()
        {
            try
            {
                _logger.log("Get all villaNumbers", "");
                IEnumerable<VillaNumber> villaNumbers = await _dbvillaNumber.GetAllAsync();
                _APIResponse.StatusCode = HttpStatusCode.OK;
                _APIResponse.Result = _mapping.Map<List<VillaNumberDTO>>(villaNumbers);
                return Ok(_APIResponse);
            }
            catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;
          
        }
        [HttpGet("{id:int}",Name ="GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.log("Get villa Number Error With Id " + id, "Error");
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo== id);
                if (villaNumber == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_APIResponse);
                }
                _logger.log("Get  vill Number ", "");
                _APIResponse.StatusCode = HttpStatusCode.OK;
                _APIResponse.Result = _mapping.Map<VillaNumberDTO>(villaNumber);
                return Ok(_APIResponse);
            }
            catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;
            
            
          
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Add([FromBody]VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (await _dbvillaNumber.GetAsync(v => v.VillaNo== villaNumberDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("Cuostm valdition", "villa Number already exist");
                    _APIResponse.EroorMessage = new List<string>
                    {
                         ModelState.ToString()
                    };
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                if(await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    ModelState.AddModelError("Cuostm valdition", "villa Id is Invaild");
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                if (villaNumberDTO == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                //if (villaDTO.Id > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}
                VillaNumber model = _mapping.Map<VillaNumber>(villaNumberDTO);

                await _dbvillaNumber.CreateAsync(model);
                _APIResponse.StatusCode = HttpStatusCode.Created;
                _APIResponse.Result = _mapping.Map<VillaNumberDTO>(model); ;
                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _APIResponse);
            }catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;
           


        }
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo== id, Tracked: false);
                if (villaNumber == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_APIResponse);
                }
                 await _dbvillaNumber.RemoveAsync(villaNumber);
                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;
           

            
        }


        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody]VillaNumberUpdateDTO villaNumberDTO)
        {
            try
            {
                if (id != villaNumberDTO.VillaNo|| villaNumberDTO == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                if (await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    ModelState.AddModelError("Cuostm valdition", "villa Id is Invaild");
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo == id, Tracked: false);
                if (villaNumber == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_APIResponse);
                }
                VillaNumber model = _mapping.Map<VillaNumber>(villaNumberDTO);

               await  _dbvillaNumber.UpdateAsync(model);
                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;
       
            


        }

        [HttpPatch("{id:int}", Name = "UpdatePartiolVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartiolVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> partiolDTO)
        {
            try
            {
                if (id == 0 || partiolDTO == null)
                {
                   
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                VillaNumber villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo== id, Tracked: false);
                if (villaNumber == null)
                {
                 
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                VillaNumberUpdateDTO villaNumberDTO = _mapping.Map<VillaNumberUpdateDTO>(villaNumber);

                partiolDTO.ApplyTo(villaNumberDTO, ModelState);
                if (await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    ModelState.AddModelError("Cuostm valdition", "villa Id is Invaild");
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                if (!ModelState.IsValid)
                {
                    _APIResponse.EroorMessage = new List<string>
                    {
                         ModelState.ToString()
                    };
                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }
                VillaNumber modle = _mapping.Map<VillaNumber>(villaNumberDTO);

                 await  _dbvillaNumber.UpdateAsync(modle);

                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }catch(Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.EroorMessage = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;


        }
    }
}
