using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/sharing")]
    public class SharingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SharingController> _logger;

        public SharingController(IMediator mediator, ILogger<SharingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSharing([FromBody] CreateSharingCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create sharing for User {UserId} and Certificate {CertificateId}", request.UserId, request.CertificateId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create sharing");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
