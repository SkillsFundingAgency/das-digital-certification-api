using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateAdminAction
{
    [TestFixture]
    public class WhenHandlingCreateAdminActionCommandHandler
    {
        private Mock<IAdminActionsEntityContext> _adminActionsContextMock = null!;
        private Mock<IUserActionsEntityContext> _userActionsContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateAdminActionCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _adminActionsContextMock = new Mock<IAdminActionsEntityContext>();
            _userActionsContextMock = new Mock<IUserActionsEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _dateTimeProviderMock.SetupGet(d => d.Now).Returns(DateTime.UtcNow);

            _sut = new CreateAdminActionCommandHandler(_adminActionsContextMock.Object, _userActionsContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_UserActionNotFound_Then_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateAdminActionCommand { Username = "admin", Action = AdminActionType.Viewed, UserActionId = 999 };

            _userActionsContextMock.Setup(x => x.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act / Assert
            await _sut.Invoking(s => s.Handle(command, CancellationToken.None)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task And_ValidUserAction_Then_AddsAdminActionAndSaves()
        {
            // Arrange
            var ua = new UserActions { Id = 1, ActionType = ActionType.Reprint, FamilyName = "Smith", GivenNames = "Test" };
            var command = new CreateAdminActionCommand { Username = "admin", Action = AdminActionType.Viewed, UserActionId = 1 };

            _userActionsContextMock.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _adminActionsContextMock.Verify(x => x.Add(It.IsAny<AdminActions>()), Times.Once);
            _adminActionsContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
