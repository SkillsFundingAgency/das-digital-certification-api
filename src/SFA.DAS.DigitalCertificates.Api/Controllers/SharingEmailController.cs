using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/sharingemail")]
    public class SharingEmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingEmailController> _logger;

        public SharingEmailController(IMediator mediator, ILogger<SharingEmailController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("emaillinkcode/{emailLinkCode}")]
        public async Task<IActionResult> GetSharingByEmailLinkCode(Guid emailLinkCode)
        {
            try
            {
                var result = await _mediator.Send(new GetSharingByEmailLinkCodeQuery
                {
                    EmailLinkCode = emailLinkCode
                });

                if (result?.SharingEmail == null)
                {
                    return NotFound();
                }

                return Ok(result.SharingEmail);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve sharing email by link code {EmailLinkCode}", emailLinkCode);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve sharing email by link code {EmailLinkCode}", emailLinkCode);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
