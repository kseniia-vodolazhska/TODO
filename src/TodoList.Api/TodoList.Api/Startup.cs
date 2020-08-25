using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Api.Data;
using TodoList.Api.Data.Models;
using TodoList.Api.Data.Repositories;
using TodoList.Api.Framework.Middleware;
using TodoList.Api.Services;

namespace TodoList.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"), 
                o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(o => o.AddPolicy("AllowAll", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            services.AddDefaultIdentity<ApplicationUser>(x => {
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddJwtAuthentication(this.Configuration.GetSection("AppSettings:Token").Value);

            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();

            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
            
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}