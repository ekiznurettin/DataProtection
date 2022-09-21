using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPI
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
            services.AddCors(options =>
            {
                //options.AddDefaultPolicy(builder =>
                //{
                //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();//tüm istekleri nerden gelirse gelsin kabul eder
                //});
                options.AddPolicy("AllowSites", builder =>
                {
                    // builder.WithOrigins("https://localhost:44309", "https://www.mysite.com:44309").AllowAnyHeader().AllowAnyMethod();//belirtilen sitelerden gelen isteklere izin verir
                    builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();//alt domainlerden gelen isteklere izin verir
                });
                //options.AddPolicy("AllowSites2", builder =>
                //{
                //    builder.WithOrigins("http://www.mysite2.com").WithHeaders(HeaderNames.ContentType, "x-custom-header");
                //});

                options.AddPolicy("AllowSites2", builder =>
                {
                    builder.WithOrigins("https://localhost:44309").WithMethods("POST", "GET").AllowAnyHeader();
                });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors();
            app.UseCors("AllowSites");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
