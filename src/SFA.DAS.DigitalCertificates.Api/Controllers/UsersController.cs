using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Api.Models;

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
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(string govUkIdentifier)
        {
            try
            {
                var result = await _mediator.Send(new GetUserQuery { GovUkIdentifier = govUkIdentifier });
                return Ok((GetUserResponse?)result.User);
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

        [HttpPost("")]
        [ProducesResponseType(typeof(CreateOrUpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            try
            {
                var result = await _mediator.Send((CreateOrUpdateUserCommand)request);
                return Ok((CreateOrUpdateUserResponse?)result);
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

        [HttpPost("{userId}/identity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserIdentity(Guid userId, [FromBody] UpdateUserIdentityRequest request)
        {
            try
            {
                await _mediator.Send(new UpdateUserIdentityCommand(request, userId));
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to update user identity.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to update user identity.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{userId}/authorisation")]
        [ProducesResponseType(typeof(GetUserAuthorisationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAuthorisation(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserAuthorisationQuery { UserId = userId });
                return Ok((GetUserAuthorisationResponse?)result);
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
        [ProducesResponseType(typeof(GetUserIdentityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserIdentity(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserIdentityQuery { UserId = userId });
                return Ok((GetUserIdentityResponse?)result);
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

        [HttpGet("id/{userId:guid}")]
        [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery { UserId = userId });
                return Ok((GetUserByIdResponse?)result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve user for {UserId}", userId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve user for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{userId}/sharings")]
        [ProducesResponseType(typeof(GetSharingsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return Ok((GetSharingsResponse?)result.SharingDetails);
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

        [HttpPost("{userId}/user-actions")]
        [ProducesResponseType(typeof(CreateUserActionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAction(Guid userId, [FromBody] CreateUserActionRequest request)
        {
            try
            {
                var command = (CreateUserActionCommand)request;
                command.UserId = userId;
                var result = await _mediator.Send(command);
                return Ok((CreateUserActionResponse?)result);
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

        [HttpGet("{userId}/user-actions")]
        [ProducesResponseType(typeof(GetUserActionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserActions(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionsQuery { UserId = userId });
                return Ok((GetUserActionsResponse?)result);
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



        [HttpPost("{userId}/match")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserMatch(Guid userId, [FromBody] CreateUserMatchRequest request)
        {
            try
            {
                var command = (CreateUserMatchCommand)request;
                command.UserId = userId;
                await _mediator.Send(command);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAuthorisation(Guid userId, [FromBody] CreateUserAuthorisationRequest request)
        {
            try
            {
                var command = (CreateUserAuthorisationCommand)request;
                command.UserId = userId;
                await _mediator.Send(command);
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

        [HttpPut("{userId}/unlock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new UnlockUserCommand { UserId = userId });

                if (result.NotFound)
                {
                    return BadRequest(new { userId });
                }

                if (result.Updated)
                {
                    return NoContent();
                }

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to unlock user for {UserId}", userId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to unlock user for {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
