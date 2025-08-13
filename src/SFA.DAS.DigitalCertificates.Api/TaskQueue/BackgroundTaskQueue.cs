using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.TaskQueue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<(IBaseRequest? Request, string RequestName, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>> response)> _requests = new ConcurrentQueue<(IBaseRequest?, string, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>>)>();
        private readonly SemaphoreSlim _signal = new(0);

        public void QueueBackgroundRequest(IBaseRequest? request, string requestName, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>> response)
        {            
            _requests.Enqueue((request, requestName, response));
            _signal.Release();
        }

        public async Task<(IBaseRequest? Request, string RequestName, Action<object?, TimeSpan, ILogger<TaskQueueHostedService>> Response)> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _requests.TryDequeue(out var request);

            return request;
        }
    }
}
