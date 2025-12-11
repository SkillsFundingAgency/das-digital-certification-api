using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{govUkIdentifier}")]
        public async Task<IActionResult> GetUser(string govUkIdentifier)
        {
            try
            {
                var result = await _mediator.Send(new GetUserQuery { GovUkIdentifier = govUkIdentifier });
                return Ok(result.User);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user for {GovUkIdentifier}", govUkIdentifier);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user for {GovUkIdentifier}", govUkIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("identity")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            try
            {
                var result = await _mediator.Send((CreateOrUpdateUserCommand)request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create or update user.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create or update user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{userId}/authorisation")]
        public async Task<IActionResult> GetUserAuthorisation(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserAuthorisationQuery { UserId = userId });
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user authorisation for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user authorisation for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{userId}/sharings")]
        public async Task<IActionResult> GetSharings(Guid userId, [FromQuery] Guid certificateId, [FromQuery] int? limit = null)
        {
            try
            {
                var result = await _mediator.Send(new GetSharingsQuery
                {
                    UserId = userId,
                    CertificateId = certificateId,
                    Limit = limit
                });

                if (result.SharingDetails == null)
                {
                    return NotFound();
                }

                return Ok(result.SharingDetails);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve sharings for User {UserId} and Certificate {CertificateId}", userId, certificateId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve sharings for User {UserId} and Certificate {CertificateId}", userId, certificateId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
