using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.ClientModels.Queries;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    ///     ClientModels If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientModelsController : BaseApiController
    {
        /// <summary>
        ///     List ClientModels
        /// </summary>
        /// <remarks>ClientModels</remarks>
        /// <return>List ClientModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<ClientModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getlastclientsbycount")]
        public async Task<IActionResult> GetLastClientsByCount(int count)
        {
            var result = await Mediator.Send(new GetLastClientsByCountQuery{ Count = count });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        ///     List ClientModels
        /// </summary>
        /// <remarks>ClientModels</remarks>
        /// <return>List ClientModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<int>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getpositivesentimentrate")]
        public async Task<IActionResult> GetPositiveSentimentRate()
        {
            var result = await Mediator.Send(new GetPositiveSentimentRateQuery());
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     List ClientModels
        /// </summary>
        /// <remarks>ClientModels</remarks>
        /// <return>List ClientModels</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<IEnumerable<int>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("gettotalclientcount")]
        public async Task<IActionResult> GetTotalClientCount()
        {
            var result = await Mediator.Send(new GetTotalClientCountQuery());
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        ///     It brings the details according to its id.
        /// </summary>
        /// <remarks>ClientModels</remarks>
        /// <return>ClientModels List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpGet("getpositivesentimentratebydate")]
        public async Task<IActionResult> GetPositiveSentimentRateByDate(int startDate, int finishDate)
        {
            var result = await Mediator.Send(new GetSentimentRateByDateFilterQuery
            {
                startDate = startDate,
                finishDate = finishDate
            });
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

    }
}