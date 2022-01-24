using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Installers
{
    public static class InstallerExtension
    {
        public static void InstallerServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            installers.ForEach(i => i.ConfigureServices(services, configuration));
        }
    }
}
