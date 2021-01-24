using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetCoreApiSample.Entity;
using DotnetCoreApiSample.Server.Infrastructure;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Services.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DotnetCoreApiSample.Server
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "TestDb");
            });

            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", options =>
                {
                    

                });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilter)); //Global model validation filter
            }).ConfigureApiBehaviorOptions(apiOptions =>
            {
                apiOptions.SuppressModelStateInvalidFilter = true; //[ApiController] attribute checks invalid request by default. Disable and check manually
            });

            services.AddHttpContextAccessor(); //Expose httpContext to services

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserPipe<,>)); //mediatR pipiline

            services.AddMediatR(typeof(BaseRequest).Assembly); //Provide Assembly where our handlers live

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cqrs.Sample", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cqrs.Sample v1"));
            }

            using var scope = app.ApplicationServices.CreateScope(); //get service scope 
            var context = scope.ServiceProvider.GetService<AppDbContext>(); //get db context
            DatabaseGenerator.Initialize(context); //seed some data

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}