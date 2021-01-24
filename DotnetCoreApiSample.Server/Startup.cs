using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoreApiSample.Entity;
using DotnetCoreApiSample.Server.Infrastructure;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Services.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

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
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = Constants.SecurtyAudience,
                        ValidateIssuer = true,
                        ValidIssuer = Constants.SecurtyIssuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SecurtyKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = ctx => Task.CompletedTask,
                        OnAuthenticationFailed = ctx =>
                        {
                            //Log exception message ctx.Exception.Message + ctx.Exception.InnerMessage?.Message
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = ctx => //If user sends access token in query parameters, use that token
                        {
                            if (ctx.Request.Query.ContainsKey("access_token"))
                                ctx.Token = ctx.Request.Query["access_token"];
                            
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy)); //Global authorization
                options.Filters.Add(new ProducesAttribute("application/json")); //Global media type support
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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cqrs.Sample", 
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "ugurrdal@gmail.com",
                        Name = "Ugur Dal",
                        Url = new Uri("https://github.com/ugurdal")
                    }
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                
                // var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
                // c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DotnetCoreApiSample.Models.xml"));
                // c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DotnetCoreApiSample.Services.xml"));
                c.AddEnumsWithValuesFixFilters(services, options =>
                {
                    options.IncludeDescriptions = true;
                });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            services.AddResponseCompression(options =>
            {
                var mimeTypes = new[]
                {
                    "text/plain",
                    "text/html",
                    "text/css",
                    "application/javascript",
                    "image/x-icon",
                    "image/png",
                    "application/json",
                    "text/json"
                };

                options.EnableForHttps = true;
                options.MimeTypes = mimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
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

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}