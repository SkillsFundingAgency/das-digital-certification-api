using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/sharingaccess")]
    public class SharingAccessController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingAccessController> _logger;

        public SharingAccessController(IMediator mediator, ILogger<SharingAccessController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharingAccess([FromBody] CreateSharingAccessCommand request)
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
                _logger.LogError(ex, "Validation error attempting to create sharing access for Sharing {SharingId}", request.SharingId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create sharing access for Sharing {SharingId}", request.SharingId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
