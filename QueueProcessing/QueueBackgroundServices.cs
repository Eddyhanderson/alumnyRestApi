using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace alumni.QueueProcessing
{
    public interface IQueueBackgroundServices
    {
        void QueueTask(Func<CancellationToken, IServiceProvider, Task> task);
        Task<Func<CancellationToken, IServiceProvider, Task>> PopQueue(CancellationToken cancellationToken);
    }

    public class QueueBackgroundServices : IQueueBackgroundServices
    {
        private readonly ILogger<QueueBackgroundServices> logger;
        private readonly ConcurrentQueue<Func<CancellationToken, IServiceProvider, Task>> fifo;
        private readonly SemaphoreSlim semaphore;

        public QueueBackgroundServices(ILogger<QueueBackgroundServices> logger)
        {
            this.logger = logger;

            fifo = new ConcurrentQueue<Func<CancellationToken, IServiceProvider, Task>>();

            semaphore = new SemaphoreSlim(0);
        }

        public void QueueTask(Func<CancellationToken, IServiceProvider, Task> task)
        {
            if (task == null) throw new ArgumentNullException();

            logger.LogInformation("Enqueue process are starting.");
            fifo.Enqueue(task);
            semaphore.Release();
            logger.LogInformation("Enqueue process are finished.");
        }

        public async Task<Func<CancellationToken, IServiceProvider, Task>> PopQueue(CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            logger.LogInformation("Dequeue process are starting.");
            fifo.TryDequeue(out var task);
            logger.LogInformation("Dequeue process are finished.");

            return task;
        }

    }
}
