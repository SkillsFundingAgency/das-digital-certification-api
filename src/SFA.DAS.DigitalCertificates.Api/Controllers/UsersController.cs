using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;

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

        [HttpGet("{userId}/identity")]
        public async Task<IActionResult> GetUserIdentity(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserIdentityQuery { UserId = userId });
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user identity for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user identity for {UserId}", userId);
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

        [HttpPost("{userId}/actions")]
        public async Task<IActionResult> CreateUserAction(Guid userId, [FromBody] CreateUserActionCommand request)
        {
            try
            {
                request.UserId = userId;
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create user action for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create user action for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("adminactions")]
        public async Task<IActionResult> CreateAdminAction([FromBody] CreateAdminActionCommand request)
        {
            try
            {
                await _mediator.Send(request);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create admin action.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create admin action.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{userId}/actions")]
        public async Task<IActionResult> GetUserActions(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionsQuery { UserId = userId });
                return Ok(new { userActions = result.UserActions });
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user actions for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user actions for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("useractions/{code}")]
        public async Task<IActionResult> GetUserActionByCode(string code)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionByCodeQuery { ActionCode = code });
                if (result.UserAction == null)
                {
                    return NotFound();
                }

                return Ok(result.UserAction);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user action for {ActionCode}", code);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user action for {ActionCode}", code);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{userId}/match")]
        public async Task<IActionResult> CreateUserMatch(Guid userId, [FromBody] CreateUserMatchCommand request)
        {
            try
            {
                request.UserId = userId;
                await _mediator.Send(request);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create user match for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create user match for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{userId}/authorise")]
        public async Task<IActionResult> CreateUserAuthorisation(Guid userId, [FromBody] CreateUserAuthorisationCommand request)
        {
            try
            {
                request.UserId = userId;
                await _mediator.Send(request);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to authorise user for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to authorise user for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
