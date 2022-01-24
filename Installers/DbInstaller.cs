using alumni.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Domain;

namespace alumni.Installers
{
    public class DbInstaller : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
            {   
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDefaultIdentity<User>(option =>{
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.User.RequireUniqueEmail = false;
                option.SignIn.RequireConfirmedAccount = false;
                option.SignIn.RequireConfirmedEmail = false;                
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
