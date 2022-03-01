using alumni.Data;
using alumni.QueueProcessing;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.BackgroundServices
{
    public class FormationEventStateMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FormationEventStateMonitorService> _logger;

        private Timer _timer = null!;

        public FormationEventStateMonitorService(IServiceProvider serviceProvider,
        ILogger<FormationEventStateMonitorService> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(SetStartedState, null, TimeSpan.FromSeconds(1), TimeSpan.FromHours(24));

            _timer = new Timer(SetFinishedState, null, TimeSpan.FromSeconds(1), TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        private void SetStartedState(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                
                dataContext.Database.ExecuteSqlRaw($"UPDATE [Alumni].[dbo].[FormationEvents] SET [Alumni].[dbo].[FormationEvents].[State] = 'Started' WHERE ([Start] < GETDATE()) AND [State] = 'Waiting' AND [Situation] = 'Normal'");             
            }

        }

        private void SetFinishedState(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dataContext.Database.ExecuteSqlRaw($"UPDATE [Alumni].[dbo].[FormationEvents] SET [Alumni].[dbo].[FormationEvents].[State] = 'Finished' WHERE ([End] < GETDATE()) AND [State] = 'Waiting' AND [Situation] = 'Normal'");             
            }
        }
    }
}
