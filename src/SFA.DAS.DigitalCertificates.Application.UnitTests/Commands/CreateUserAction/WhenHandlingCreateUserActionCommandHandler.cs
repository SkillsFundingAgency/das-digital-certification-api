using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Data;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.Encoding;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAction
{
    public class WhenHandlingCreateUserActionCommandHandler
    {
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private Mock<IEncodingService> _encodingServiceMock = null!;

        [SetUp]
        public void SetUp()
        {
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _encodingServiceMock = new Mock<IEncodingService>();
        }

        [Test]
        public async Task And_NoExistingAction_Then_CreatesNewActionAndReturnsNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var expectedCode = "ACTION123";

            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<DigitalCertificatesDataContext>()
                .UseSqlite(connection)
                .Options;

            await using var dbContext = new DigitalCertificatesDataContext(null, options);
            dbContext.Database.EnsureCreated();

            dbContext.Users.Add(new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com" });
            await dbContext.SaveChangesAsync();

            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _encodingServiceMock.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.SupportReference)).Returns(expectedCode);

            var sut = new CreateUserActionCommandHandler(dbContext, _dateTimeProviderMock.Object, _encodingServiceMock.Object);

            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.ActionCode.Should().Be(expectedCode);
            var saved = await dbContext.UserActions.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.ActionCode.Should().Be(expectedCode);
        }

        [Test]
        public async Task And_ExistingActionWithNoAdminActions_Then_ReturnsExistingCodeWithNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingCode = "EXISTING123";

            var existing = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                ActionCode = existingCode,
                ActionTime = DateTime.UtcNow,
                AdminActions = new List<AdminActions>()
            };

            using var connection2 = new SqliteConnection("DataSource=:memory:");
            connection2.Open();
            var options2 = new DbContextOptionsBuilder<DigitalCertificatesDataContext>()
                .UseSqlite(connection2)
                .Options;

            await using (var dbContext = new DigitalCertificatesDataContext(null, options2))
            {
                dbContext.Database.EnsureCreated();

                dbContext.Users.Add(new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com" });
                await dbContext.SaveChangesAsync();

                var certificateId = Guid.NewGuid();

                existing.UserId = userId;
                existing.ActionType = ActionType.Reprint;
                existing.CertificateId = certificateId;
                dbContext.UserActions.Add(existing);
                await dbContext.SaveChangesAsync();

                var sut = new CreateUserActionCommandHandler(dbContext, _dateTimeProviderMock.Object, _encodingServiceMock.Object);

                var command = new CreateUserActionCommand
                {
                    UserId = userId,
                    ActionType = ActionType.Reprint,
                    FamilyName = "Smith",
                    GivenNames = "John",
                    CertificateId = certificateId,
                    CertificateType = CertificateType.Standard,
                    CourseName = "Test Course"
                };

                // Act
                var result = await sut.Handle(command, CancellationToken.None);

                // Assert
                result.ActionCode.Should().Be(existingCode);
                var count = await dbContext.UserActions.CountAsync();
                count.Should().Be(1);
            }
        }

        [Test]
        public async Task And_ExistingActionWithAdminActions_Then_CreatesNewActionAndReturnsNewStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var existingCode = "EXISTING123";
            var newCode = "NEW456";

            var existing = new UserActions
            {
                Id = 1,
                UserId = userId,
                ActionType = ActionType.Help,
                FamilyName = "Smith",
                GivenNames = "John",
                ActionCode = existingCode,
                ActionTime = DateTime.UtcNow,
                AdminActions = new List<AdminActions>
                {
                    new AdminActions { Id = Guid.NewGuid(), Username = "admin", Action = AdminActionType.Viewed, ActionTime = DateTime.UtcNow, UserActionId = 1 }
                }
            };

            using var connection3 = new SqliteConnection("DataSource=:memory:");
            connection3.Open();
            var options3 = new DbContextOptionsBuilder<DigitalCertificatesDataContext>()
                .UseSqlite(connection3)
                .Options;

            await using (var dbContext = new DigitalCertificatesDataContext(null, options3))
            {
                dbContext.Database.EnsureCreated();

                dbContext.Users.Add(new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com" });
                await dbContext.SaveChangesAsync();

                existing.UserId = userId;
                existing.ActionType = ActionType.Help;
                existing.CertificateId = certificateId;
                dbContext.UserActions.Add(existing);
                await dbContext.SaveChangesAsync();

                _dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
                _encodingServiceMock.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.SupportReference)).Returns(newCode);

                var sut = new CreateUserActionCommandHandler(dbContext, _dateTimeProviderMock.Object, _encodingServiceMock.Object);

                var command = new CreateUserActionCommand
                {
                    UserId = userId,
                    ActionType = ActionType.Help,
                    FamilyName = "Smith",
                    GivenNames = "John",
                    CertificateId = certificateId,
                    CertificateType = CertificateType.Standard,
                    CourseName = "Test Course"
                };

                // Act
                var result = await sut.Handle(command, CancellationToken.None);

                // Assert
                result.ActionCode.Should().Be(newCode);
                var count = await dbContext.UserActions.CountAsync();
                count.Should().Be(2);
                var saved = await dbContext.UserActions.OrderByDescending(u => u.Id).FirstAsync();
                saved.ActionCode.Should().Be(newCode);
            }
        }
    }
}
