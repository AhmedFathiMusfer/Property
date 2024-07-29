using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;

namespace Property_WepAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVills()
        {
            return Ok( VillaStore.VillaList);
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVill(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa) ;
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<VillaDTO> Add([FromBody]VillaDTO villaDTO)
        {
            if (villaDTO==null)
            {
              
                return BadRequest();
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla",new {id= villaDTO.Id }, villaDTO);


        }
    }
}
