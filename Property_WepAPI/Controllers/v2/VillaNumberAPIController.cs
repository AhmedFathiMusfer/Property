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

        protected APIResponse _APIResponse;


        public VillaNumberAPController(ILogging logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapping)
        {
            _logger = logger;
            _dbvillaNumber = dbVillaNumber;
            _mapping = mapping;
            _dbVilla = dbVilla;
            _APIResponse = new();
        }

       
    }
}
