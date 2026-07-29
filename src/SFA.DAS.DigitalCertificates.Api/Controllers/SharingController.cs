using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode;
using System;
using System.Threading.Tasks;
using SFA.DAS.DigitalCertificates.Api.Models;

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

        [HttpGet("sharingemail/emaillinkcode/{emailLinkCode}")]
        [ProducesResponseType(typeof(GetSharingByEmailLinkCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return Ok((GetSharingByEmailLinkCodeResponse?)result.SharingEmail);
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

        [HttpGet("linkcode/{linkCode}")]
        [ProducesResponseType(typeof(GetSharingByLinkCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return Ok((GetSharingByLinkCodeResponse?)result.Sharing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve sharing by link code {LinkCode}", linkCode);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetSharingByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return Ok((GetSharingByIdResponse?)result.Sharing);
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
        [ProducesResponseType(typeof(CreateSharingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSharing([FromBody] CreateSharingRequest request)
        {
            try
            {
                var command = (CreateSharingCommand)request;
                var result = await _mediator.Send(command);
                return Ok((CreateSharingResponse?)result);
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
        [ProducesResponseType(typeof(CreateSharingEmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSharingEmail(Guid id, [FromBody] CreateSharingEmailRequest request)
        {
            try
            {
                var command = (CreateSharingEmailCommand)request;
                command.SharingId = id;
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((CreateSharingEmailResponse?)result);
            }
            catch (ValidationException ex)
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

        [HttpPost("sharingaccess")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSharingAccess([FromBody] CreateSharingAccessRequest request)
        {
            try
            {
                var command = (CreateSharingAccessCommand)request;
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (ValidationException ex)
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

        [HttpPost("sharingemailaccess")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSharingEmailAccess([FromBody] CreateSharingEmailAccessRequest request)
        {
            try
            {
                var command = (CreateSharingEmailAccessCommand)request;
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (ValidationException ex)
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    return NotFound();
                }

                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to delete sharing {SharingId}", id);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to delete sharing {SharingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
