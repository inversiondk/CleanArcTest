using System.Net;
using CleanArcTest.Application.Trademarks.Queries;
using CleanArcTest.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrademarksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrademarksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Trademark>), (int)HttpStatusCode.OK)]
        [Route("GetTrademarks")]
        public async Task<IActionResult> Get(CancellationToken token = new ())
        {
            var data = await _mediator.Send(new GetTrademarksQuery(), token);
            return Ok(data);
        }
    }
}
