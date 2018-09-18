using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using POC.Api.Infrastructure.Filters;
using POC.Core.Logging;
using POC.Core.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace POC.Api
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
            ConfigureJwtAuthService(services);

            ConfigureAddMVC(services);

            // Add default versioning
            services.AddApiVersioning();

            // Configure AutoMapper and assert that our configuration is valid.
            services.AddAutoMapper();

            // Add Swagger API documentation generator
            ConfigureAddSwagger(services);

        }


        // This method gets called by the runtime. 
        // Use this method to configure the HTTP REQUEST PIPELINE.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Use Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "POC API");
            });

            app.UseExceptionHandler(eApp =>
            {
                eApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var ex = errorCtx.Error;
                        WebHelper.LogWebError("ToDos", "Core API", ex, context);

                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var jsonResponse = JsonConvert.SerializeObject(new CustomErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "Some kind of error happened in the API."
                        });
                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStatusCodePages();



            //Add Auto mapper
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<string, int>();
            });

            app.UseMvc();
        }


        #region private helpers

        /// <summary>
        /// Add MVC
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureAddMVC(IServiceCollection services)
        {
            services.AddMvc(config => {
                // Make authentication compulsory across the board (i.e. shut
                // down EVERYTHING unless explicitly opened up).
                var policy = new AuthorizationPolicyBuilder()
                          .RequireAuthenticatedUser()
                          .Build();
                config.Filters.Add(new AuthorizeFilter(policy));

                //Add Performance Filter
                config.Filters.Add(new TrackPerformanceFilter("API", "Core API"));
            })
            .AddMvcOptions(o => {
                //Output formatter to accept a response serialization as xml.  Json is already included.
                o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            })
            .AddJsonOptions(o => {

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// Configure JWT Auth Service
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureJwtAuthService(IServiceCollection services)
        {

            var audienceConfig = Configuration.GetSection("JWTAuth");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var validIssuer = audienceConfig["Issuer"];
            var ValidAudience = audienceConfig["Audience"];

            //Configure Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {  //Options should match what's in the auth controller
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = validIssuer,
                        ValidAudience = ValidAudience,
                        IssuerSigningKey = signingKey
                    };
                });
        }

        /// <summary>
        /// Add and configure swagger service
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureAddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "POC API", Version = "v1" });
                //var authority = Environment.GetEnvironmentVariable("AUTHORITY");
                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "implicit",
                //    AuthorizationUrl = $"{authority}/connect/authorize",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        { "api", "Access to the API" },
                //    }
                //});

                c.OperationFilter<SwaggerSecurityOperationFilter>("api");

            });
        }
        #endregion
    }
}
