using alumni.QueueProcessing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace alumni.BackgroundServices
{
    public class QueueTaskExecutionBackgroundService : BackgroundService
    {
        private readonly IQueueBackgroundServices queue;
        private readonly IServiceProvider serviceProvider;

        public QueueTaskExecutionBackgroundService(IQueueBackgroundServices queue, IServiceProvider serviceProvider)
        {
            this.queue = queue;
            this.serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await queue.PopQueue(stoppingToken);

                await task(stoppingToken, serviceProvider);
            }
        }
    }
}
