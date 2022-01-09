using System.Threading.Tasks;
using Business.Handlers.ApiInfoModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     ApiInfoModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiInfoModelsController : BaseApiController
    {
        /// <summary>
        ///     It brings the details according to its id.
        /// </summary>
        /// <remarks>MlInfoModels</remarks>
        /// <return>MlInfoModels List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<ApiInfoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IDataResult<ApiInfoModel>))]
        [HttpGet("getbytype")]
        public async Task<IActionResult> GetById(string type)
        {
            var result = await Mediator.Send(new GetApiInfoModelByTypeQuery {Type = type});
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}