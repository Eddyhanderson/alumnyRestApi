
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Installers
{
    interface IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
