using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

namespace Property_WepAPI.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/VillaAPI")]
    [ApiController]
    [ApiVersion("1.0")]
   
    [Authorize]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapping;
        private readonly ILogging _logger;

        protected APIResponse _APIResponse;


        public VillaAPIController(ILogging logger, IVillaRepository dbVilla, IMapper mapping)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapping = mapping;
            _APIResponse = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
  
        public async Task<ActionResult<APIResponse>> GetVills()
        {
            try
            {
                _logger.log("Get all villas", "");
                IEnumerable<Villa> villas = await _dbVilla.GetAllAsync();
                _APIResponse.StatusCode = HttpStatusCode.OK;
                _APIResponse.Result = _mapping.Map<List<VillaDTO>>(villas);
                return Ok(_APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;

        }
    
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<ActionResult<APIResponse>> GetVill(int id)
        {
            try
            {
                if (id == 0)
                {


                    return BadRequest(_APIResponse);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id);
                if (villa == null)
                {

                    return NotFound(_APIResponse);
                }


                _APIResponse.Result = _mapping.Map<VillaDTO>(villa);
                return Ok(_APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
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
        public async Task<ActionResult<APIResponse>> Add([FromBody] VillaCreateDTO villaDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "villa already exist");

                    return BadRequest(ModelState);
                }
                if (villaDTO == null)
                {
                    return BadRequest(_APIResponse);
                }

                Villa model = _mapping.Map<Villa>(villaDTO);

                await _dbVilla.CreateAsync(model);
                _APIResponse.StatusCode = HttpStatusCode.Created;
                _APIResponse.Result = _mapping.Map<VillaDTO>(model); ;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;



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

                    return BadRequest(_APIResponse);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {

                    return NotFound(_APIResponse);
                }
                await _dbVilla.RemoveAsync(villa);
                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;



        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (id != villaDTO.Id || villaDTO == null)
                {

                    return BadRequest(_APIResponse);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {

                    return NotFound(_APIResponse);
                }
                Villa model = _mapping.Map<Villa>(villaDTO);

                await _dbVilla.UpdateAsync(model);
                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;




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


                    return BadRequest(_APIResponse);
                }
                Villa villa = await _dbVilla.GetAsync(v => v.Id == id, Tracked: false);
                if (villa == null)
                {


                    return BadRequest(_APIResponse);
                }
                VillaUpdateDTO villaDTO = _mapping.Map<VillaUpdateDTO>(villa);

                partiolDTO.ApplyTo(villaDTO, ModelState);
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                Villa modle = _mapping.Map<Villa>(villaDTO);

                await _dbVilla.UpdateAsync(modle);

                _APIResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_APIResponse);
            }
            catch (Exception e)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>
                {
                    e.ToString()
                };
            }
            return _APIResponse;


        }
    }
}
