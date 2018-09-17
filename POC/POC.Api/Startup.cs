using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

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
            //TODO: This comes from a config file. 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fasfsadfasdfsdfasdfasdfsd"));

            //Configure Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {  //Options should match what's in the auth controller
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "mysite.com",
                        ValidAudience = "mysite.com",
                        IssuerSigningKey = key
                    };
                });

            services.AddMvc(config => {
                // Make authentication compulsory across the board (i.e. shut
                // down EVERYTHING unless explicitly opened up).
                var policy = new AuthorizationPolicyBuilder()
                          .RequireAuthenticatedUser()
                          .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddMvcOptions(o => {
                    //Output formatter to accept a response serialization as xml.  Json is already included.
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                })
                .AddJsonOptions(o => {

                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddApiVersioning();

            // Configure AutoMapper and assert that our configuration is valid.
            services.AddAutoMapper();
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

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStatusCodePages();

            //Add Auto mapper
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<string, int>();
            });

            app.UseMvc();
        }
    }
}
