using System.Net;
using CleanArcTest.Api.Models;
using CleanArcTest.Application.Registrations.Queries;
using CleanArcTest.Application.Registrations.Queries.DTOs;
using CleanArcTest.Application.Trademarks.Commands.AddRegistration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RegistrationDto>), (int)HttpStatusCode.OK)]
        [Route("GetRegistrations/{trademarkId}")]
        public async Task<IActionResult> Get(int trademarkId, CancellationToken token = new ())
        {
            var data = await _mediator.Send(new GetRegistrationsQuery(trademarkId), token);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("AddRegistration")]
        public async Task<IActionResult> AddRegistration([FromBody] AddRegistrationModel model, CancellationToken token = new())
        {
            var data = await _mediator.Send(new AddRegistrationCommand(model.TrademarkId, model.CountryIso, model.RenewalPrice), token);
            if (data)
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
