using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/sharingemailaccess")]
    public class SharingEmailAccessController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingEmailAccessController> _logger;

        public SharingEmailAccessController(IMediator mediator, ILogger<SharingEmailAccessController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharingEmailAccess([FromBody] CreateSharingEmailAccessCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);

                if (result == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create sharing email access for SharingEmail {SharingEmailId}", request.SharingEmailId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create sharing email access for SharingEmail {SharingEmailId}", request.SharingEmailId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
