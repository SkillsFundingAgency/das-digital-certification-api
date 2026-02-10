using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenHandlingGetSharingByIdQueryHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private GetSharingByIdQueryHandler _sut = null!;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(_now);
            _sut = new GetSharingByIdQueryHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNullSharing()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetSharingById(sharingId, _now)).ReturnsAsync((Sharing?)null);

            var query = new GetSharingByIdQuery { SharingId = sharingId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Sharing.Should().BeNull();
        }

        [Test]
        public async Task And_SharingExists_Then_ReturnsSharingWithCorrectData()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var linkCode = Guid.NewGuid();
            var createdAt = DateTime.UtcNow.AddDays(-1);
            var expiryTime = DateTime.UtcNow.AddDays(1);

            var sharing = new Sharing
            {
                Id = sharingId,
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                LinkCode = linkCode,
                CreatedAt = createdAt,
                ExpiryTime = expiryTime,
                Status = SharingStatus.Live
            };

            var allSharingsForUser = new List<Sharing>
            {
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certificateId, CertificateType = CertificateType.Standard, CourseName = "Test Course", LinkCode = Guid.NewGuid(), CreatedAt = createdAt.AddDays(-1), ExpiryTime = expiryTime, Status = SharingStatus.Live },
                sharing
            };

            _sharingContextMock.Setup(x => x.GetSharingById(sharingId, _now)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.GetAllSharingsBasic(userId, certificateId)).ReturnsAsync(allSharingsForUser);

            var query = new GetSharingByIdQuery { SharingId = sharingId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Sharing.Should().NotBeNull();
            result.Sharing!.UserId.Should().Be(userId);
            result.Sharing.CertificateId.Should().Be(certificateId);
            result.Sharing.CertificateType.Should().Be(CertificateType.Standard);
            result.Sharing.CourseName.Should().Be("Test Course");
            result.Sharing.SharingId.Should().Be(sharingId);
            result.Sharing.SharingNumber.Should().Be(2); // Second sharing created
            result.Sharing.CreatedAt.Should().Be(createdAt);
            result.Sharing.LinkCode.Should().Be(linkCode);
            result.Sharing.ExpiryTime.Should().Be(expiryTime);
        }

        [Test]
        public async Task And_SharingHasAccessAndEmails_Then_ReturnsWithAccessAndEmailData()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var accessTime1 = DateTime.UtcNow.AddHours(-2);
            var accessTime2 = DateTime.UtcNow.AddHours(-1);
            var sentTime = DateTime.UtcNow.AddDays(-1);

            var sharing = new Sharing
            {
                Id = sharingId,
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                LinkCode = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                Status = SharingStatus.Live,
                SharingAccesses = new List<SharingAccess>
                {
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime1, Sharing = null! },
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime2, Sharing = null! }
                },
                SharingEmails = new List<SharingEmail>
                {
                    new SharingEmail
                    {
                        Id = Guid.NewGuid(),
                        SharingId = sharingId,
                        EmailAddress = "test@example.com",
                        EmailLinkCode = Guid.NewGuid(),
                        SentTime = sentTime,
                        Sharing = null!,
                        SharingEmailAccesses = new List<SharingEmailAccess>
                        {
                            new SharingEmailAccess { Id = Guid.NewGuid(), SharingEmailId = Guid.NewGuid(), AccessedAt = accessTime1, SharingEmail = null! }
                        }
                    }
                }
            };

            _sharingContextMock.Setup(x => x.GetSharingById(sharingId, _now)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.GetAllSharingsBasic(userId, certificateId)).ReturnsAsync(new List<Sharing> { sharing });

            var query = new GetSharingByIdQuery { SharingId = sharingId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Sharing.Should().NotBeNull();
            result.Sharing!.SharingAccess.Should().HaveCount(2);
            result.Sharing.SharingAccess.Should().Contain(accessTime1);
            result.Sharing.SharingAccess.Should().Contain(accessTime2);

            result.Sharing.SharingEmails.Should().HaveCount(1);
            result.Sharing.SharingEmails![0].EmailAddress.Should().Be("test@example.com");
            result.Sharing.SharingEmails[0].SharingEmailAccess.Should().HaveCount(1);
        }

        [Test]
        public async Task And_LimitIsApplied_Then_LimitsAccessRecords()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var limit = 1;
            var accessTime1 = DateTime.UtcNow.AddHours(-2);
            var accessTime2 = DateTime.UtcNow.AddHours(-1);

            var sharing = new Sharing
            {
                Id = sharingId,
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                LinkCode = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                Status = SharingStatus.Live,
                SharingAccesses = new List<SharingAccess>
                {
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime1, Sharing = null! },
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime2, Sharing = null! }
                }
            };

            _sharingContextMock.Setup(x => x.GetSharingById(sharingId, _now)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.GetAllSharingsBasic(userId, certificateId)).ReturnsAsync(new List<Sharing> { sharing });

            var query = new GetSharingByIdQuery { SharingId = sharingId, Limit = limit };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Sharing.Should().NotBeNull();
            result.Sharing!.SharingAccess.Should().HaveCount(1);
        }

        [Test]
        public async Task And_NoLimitProvided_Then_ReturnsAllAccessRecords()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var accessTime1 = DateTime.UtcNow.AddHours(-2);
            var accessTime2 = DateTime.UtcNow.AddHours(-1);

            var sharing = new Sharing
            {
                Id = sharingId,
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                LinkCode = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpiryTime = DateTime.UtcNow.AddDays(1),
                Status = SharingStatus.Live,
                SharingAccesses = new List<SharingAccess>
                {
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime1, Sharing = null! },
                    new SharingAccess { Id = Guid.NewGuid(), SharingId = sharingId, AccessedAt = accessTime2, Sharing = null! }
                }
            };

            _sharingContextMock.Setup(x => x.GetSharingById(sharingId, _now)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.GetAllSharingsBasic(userId, certificateId)).ReturnsAsync(new List<Sharing> { sharing });

            var query = new GetSharingByIdQuery { SharingId = sharingId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Sharing.Should().NotBeNull();
            result.Sharing!.SharingAccess.Should().HaveCount(2);
        }
    }
}