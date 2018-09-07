using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vidconfile.Constants;
using Vidconfile.Data;
using Vidconfile.Data.Contracts;
using Vidconfile.Services;
using Vidconfile.Services.Services;
using Vidconfile.Utils;
using Vidconfile.Utils.Contracts;
using AutoMapper;

namespace Vidconfile
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VidconfileDBContext>(x => x.UseSqlite(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(this.Configuration.GetSection(AppSettingsConstants.Token).Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper();

            //Utils
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            //Services
            services.AddScoped<IVidconfileUserServices, VidconfileUserServices>();
            services.AddScoped<IVideoServices, VideoServices>();

            //Data
            services.AddScoped<IVidconfileDBContext, VidconfileDBContext>();
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
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
                app.UseExceptionHandler();
                //app.UseExceptionHandler(builder =>
                //{
                //    builder.Run(async context =>
                //    {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //        var error = context.Features.Get<IExceptionHandlerFeature>();

                //        if (error != null)
                //        {
                //            await context.Response.WriteAsync(error.Error.Message);
                //        }
                //    });
                //});
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
