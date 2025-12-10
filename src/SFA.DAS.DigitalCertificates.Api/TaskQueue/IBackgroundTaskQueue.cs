using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.DigitalCertificates.Api.TaskQueue
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundRequest(IBaseRequest? request, string requestName, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>> response);

        Task<(IBaseRequest? Request, string RequestName, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>> Response)> DequeueAsync(CancellationToken cancellationToken);
    }
}
