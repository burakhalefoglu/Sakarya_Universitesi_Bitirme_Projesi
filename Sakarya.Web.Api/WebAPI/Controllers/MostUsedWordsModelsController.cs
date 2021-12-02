using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.MostUsedWordsModels.Commands;
using Business.Handlers.MostUsedWordsModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     MostUsedWordsModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MostUsedWordsModelsController : BaseApiController
    {
        /// <summary>
        ///     List MostUsedWordsModels
        /// </summary>
        /// <remarks>MostUsedWordsModels</remarks>
        /// <return>List MostUsedWordsModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<MostUsedWordsModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetMostUsedWordsModelsQuery());
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     It brings the details according to its id.
        /// </summary>
        /// <remarks>MostUsedWordsModels</remarks>
        /// <return>MostUsedWordsModels List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<MostUsedWordsModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(DateTime dateTime)
        {
            var result = await Mediator.Send(new GetMostUsedWordsModelQuery {DateTime = dateTime});
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     Add MostUsedWordsModel.
        /// </summary>
        /// <param name="createMostUsedWordsModel"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMostUsedWordsModelCommand createMostUsedWordsModel)
        {
            var result = await Mediator.Send(createMostUsedWordsModel);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}