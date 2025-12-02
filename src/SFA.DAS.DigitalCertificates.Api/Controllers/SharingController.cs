using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails;

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

        [HttpGet]
        public async Task<IActionResult> GetCertificateSharingDetails([FromQuery] Guid user, [FromQuery] Guid certificateId, [FromQuery] int? limit = null)
        {
            try
            {
                var result = await _mediator.Send(new GetCertificateSharingDetailsQuery
                {
                    UserId = user,
                    CertificateId = certificateId,
                    Limit = limit
                });

                if (result.SharingDetails == null)
                {
                    _logger.LogWarning("User {UserId} not found", user);
                    return BadRequest(new { error = $"User {user} unknown" });
                }

                return Ok(result.SharingDetails);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve certificate sharing details for User {UserId} and Certificate {CertificateId}", user, certificateId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve certificate sharing details for User {UserId} and Certificate {CertificateId}", user, certificateId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificateSharing([FromBody] CreateCertificateSharingCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create certificate sharing for User {UserId} and Certificate {CertificateId}", request.UserId, request.CertificateId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create certificate sharing");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
