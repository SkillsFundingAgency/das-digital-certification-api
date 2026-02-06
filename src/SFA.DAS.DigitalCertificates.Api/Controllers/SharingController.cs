using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode;
using System;
using System.Threading.Tasks;

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

        [HttpGet("linkcode/{linkCode}")]
        public async Task<IActionResult> GetSharingByLinkCode(Guid linkCode)
        {
            try
            {
                var result = await _mediator.Send(new GetSharingByLinkCodeQuery
                {
                    LinkCode = linkCode
                });

                if (result?.Sharing == null)
                {
                    return NotFound();
                }

                return Ok(result.Sharing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve sharing by link code {LinkCode}", linkCode);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSharingById(Guid id, [FromQuery] int? limit = null)
        {
            try
            {
                var result = await _mediator.Send(new GetSharingByIdQuery
                {
                    SharingId = id,
                    Limit = limit
                });

                if (result.Sharing == null)
                {
                    return NotFound();
                }

                return Ok(result.Sharing);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve sharing by id {SharingId}", id);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve sharing by id {SharingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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

        [HttpPost("{id}/email")]
        public async Task<IActionResult> CreateSharingEmail(Guid id, [FromBody] CreateSharingEmailCommand request)
        {
            try
            {
                request.SharingId = id;
                var result = await _mediator.Send(request);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create sharing email for Sharing {SharingId}", id);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create sharing email for Sharing {SharingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSharing(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteSharingCommand
                {
                    SharingId = id
                });

                if (result == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to delete sharing {SharingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
