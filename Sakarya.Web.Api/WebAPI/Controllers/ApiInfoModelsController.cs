
using Business.Handlers.ApiInfoModels.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
namespace WebAPI.Controllers
{
    /// <summary>
    /// MlInfoModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiInfoModelsController : BaseApiController
    {
      
        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>MlInfoModels</remarks>
        ///<return>MlInfoModels List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiInfoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbytype")]
        public async Task<IActionResult> GetById(string type)
        {
            var result = await Mediator.Send(new GetApiInfoModelQuery { Type = type });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
