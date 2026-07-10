using System;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;
using SFA.DAS.DigitalCertificates.Api.Models;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("api/user-actions")]
    public class UserActionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserActionsController> _logger;

        public UserActionsController(IMediator mediator, ILogger<UserActionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("{userActionId}/admin-actions")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdminAction(long userActionId, [FromBody] CreateAdminActionRequest request)
        {
            try
            {
                var command = new CreateAdminActionCommand
                {
                    Username = request.Username,
                    Action = request.Action,
                    UserActionId = userActionId
                };

                await _mediator.Send(command);
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

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(GetUserActionByCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserActionByCode(string code)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionByCodeQuery { ActionCode = code });
                if (result == null)
                {
                    return NotFound();
                }

                GetUserActionByCodeResponse response = result;

                return Ok(response);
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
    }
}
