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

namespace Property_WepAPI.Controllers.v2
{

    [Route("api/v{version:ApiVersion}/VillaNumberAPI")]
    [ApiController]
   
    [ApiVersion("2.0")]
    public class VillaNumberAPController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbvillaNumber;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapping;
        private readonly ILogging _logger;

        protected APIResponse _response;


        public VillaNumberAPController(ILogging logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapping)
        {
            _logger = logger;
            _dbvillaNumber = dbVillaNumber;
            _mapping = mapping;
            _dbVilla = dbVilla;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> villaNumbers = await _dbvillaNumber.GetAllAsync(includeProperties: "villa");
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapping.Map<List<VillaNumberDTO>>(villaNumbers);
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
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error The ID = 0");
                    return BadRequest(_response); ;
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo == id, includeProperties: "villa");
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Number Is not Found");
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapping.Map<VillaNumberDTO>(villaNumber);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Add([FromBody] VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (await _dbvillaNumber.GetAsync(v => v.VillaNo == villaNumberDTO.VillaNo) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("villa Number already exist");
                    return BadRequest(_response); ;


                }
                if (await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("villa Id is Invaild");
                    return BadRequest(_response);
                }
                if (villaNumberDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }

                VillaNumber model = _mapping.Map<VillaNumber>(villaNumberDTO);
                await _dbvillaNumber.CreateAsync(model);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = _mapping.Map<VillaNumberDTO>(model); ;
                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _response);
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
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error The ID = 0");
                    return BadRequest(_response);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo == id, Tracked: false);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Number Is not Found");
                    return NotFound(_response);
                }
                await _dbvillaNumber.RemoveAsync(villaNumber);
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


        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villaNumberDTO)
        {
            try
            {
                if (id != villaNumberDTO.VillaNo || villaNumberDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }
                if (await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("villa Id is Invaild");
                    return BadRequest(_response);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo == id, Tracked: false);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Number  Is not Found");
                    return NotFound(_response);
                }
                VillaNumber model = _mapping.Map<VillaNumber>(villaNumberDTO);
                await _dbvillaNumber.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartiolVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartiolVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> partiolDTO)
        {
            try
            {
                if (id == 0 || partiolDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }
                VillaNumber villaNumber = await _dbvillaNumber.GetAsync(v => v.VillaNo == id, Tracked: false);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Villa Number  Is not Found");
                    return NotFound(_response);
                }
                VillaNumberUpdateDTO villaNumberDTO = _mapping.Map<VillaNumberUpdateDTO>(villaNumber);

                partiolDTO.ApplyTo(villaNumberDTO, ModelState);
                if (await _dbVilla.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("villa Id is Invaild");
                    return BadRequest(_response);
                }
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The Data Entiry is null");
                    return BadRequest(_response);
                }
                VillaNumber modle = _mapping.Map<VillaNumber>(villaNumberDTO);

                await _dbvillaNumber.UpdateAsync(modle);
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
