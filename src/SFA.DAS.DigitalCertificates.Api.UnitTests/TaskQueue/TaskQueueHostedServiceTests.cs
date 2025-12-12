using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.TaskQueue;


namespace SFA.DAS.DigitalCertificates.Api.UnitTests.TaskQueue
{
    [TestFixture]
    public class TaskQueueHostedServiceTests
    {
        private TaskQueueHostedService _service = null!;
        private Mock<IBackgroundTaskQueue> _backgroundTaskQueueMock = null!;
        private Mock<ILogger<TaskQueueHostedService>> _loggerMock = null!;
        private Mock<IServiceProvider> _serviceProviderMock = null!;
        private Mock<IServiceScopeFactory> _serviceScopeFactoryMock = null!;
        private Mock<IServiceScope> _serviceScopeMock = null!;
        private Mock<IMediator> _mediatorMock = null!;

        [SetUp]
        public void SetUp()
        {
            _backgroundTaskQueueMock = new Mock<IBackgroundTaskQueue>();
            _loggerMock = new Mock<ILogger<TaskQueueHostedService>>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _mediatorMock = new Mock<IMediator>();

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);

            _serviceScopeFactoryMock
                .Setup(x => x.CreateScope())
                .Returns(_serviceScopeMock.Object);

            _serviceScopeMock
                .Setup(x => x.ServiceProvider)
                .Returns(_serviceProviderMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IMediator)))
                .Returns(_mediatorMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(ILogger<TaskQueueHostedService>)))
                .Returns(_loggerMock.Object);

            _service = new TaskQueueHostedService(
                _backgroundTaskQueueMock.Object,
                _loggerMock.Object,
                _serviceProviderMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_DequeuesAndExecutesTasks()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var mockRequest = new Mock<IBaseRequest>().Object;

            _backgroundTaskQueueMock
                .Setup(x => x.DequeueAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((mockRequest, "TestRequest", (response, duration, logger) => { }
            ))
                .Callback(() => cancellationTokenSource.Cancel());

            // Act
            await _service.StartAsync(cancellationTokenSource.Token);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<IBaseRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _serviceScopeFactoryMock.Verify(x => x.CreateScope(), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_HandlesExceptionsGracefully()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            _backgroundTaskQueueMock
                .Setup(x => x.DequeueAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, "FailingRequest", (response, duration, logger) => throw new Exception("Simulated failure")))
                .Callback(() => cancellationTokenSource.Cancel()); // Simulate a task that fails

            // Act
            Func<Task> act = async () => await _service.StartAsync(cancellationTokenSource.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}
