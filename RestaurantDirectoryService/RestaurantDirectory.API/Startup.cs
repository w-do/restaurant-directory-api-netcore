using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RestaurantDirectory.Command;
using RestaurantDirectory.Command.Commands.City;
using RestaurantDirectory.Query.Queries.City;
using System.Data;

namespace RestaurantDirectory.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionString = Configuration.GetConnectionString("RestaurantDbConnection");
            services.AddDbContext<RestaurantDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Transient);
            services.AddScoped<IDbConnection, NpgsqlConnection>(serviceProvider => new NpgsqlConnection(connectionString));

            services.AddMediatR(typeof(AddCity).Assembly, typeof(GetCities).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x.AllowAnyMethod()
                .WithOrigins(Configuration.GetValue<string>("WebOrigin"))
                //.AllowAnyOrigin()
                .AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
