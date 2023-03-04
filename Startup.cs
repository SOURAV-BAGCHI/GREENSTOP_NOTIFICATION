using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Notification.API.CommonMethod;
using Notification.API.Data;
using Notification.API.Helpers;
using Notification.API.Implementation;
using Notification.API.Service;

namespace Notification.API
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
            services.AddControllers();
            services.AddCors(options=>{
                options.AddPolicy("EnableCORS",builder=>{
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();//AllowCredentials().Build();
                });

            });
            services.Configure<PushNotificationsOptions>(Configuration.GetSection("PushNotifications"));
            services.AddSingleton<IPushSubscriptionsService,PushSubscriptionsService>();
            services.AddSingleton<IJwtDecoder,JwtDecoder>();
            services.AddSingleton<IPushServiceClientImp,PushServiceClientImp>();
            services.AddSingleton<INotificationsProducer,NotificationsProducer>();
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            // Configure strongly typed settings objects
            var appSettingsSection=Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings=appSettingsSection.Get<AppSettings>();
            var key=Encoding.ASCII.GetBytes(appSettings.Secret);
            
            services.AddAuthentication(o=>{
                o.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme=JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;  
            }).AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,options=>{
                options.TokenValidationParameters=new TokenValidationParameters{
                    ValidateIssuerSigningKey=true,
                    ValidateIssuer=true,
                    ValidateAudience=true,
                   // RequireExpirationTime=false,
                    ValidIssuer=appSettings.Site,
                    ValidAudience=appSettings.Audience,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                    ClockSkew=TimeSpan.Zero
                };
            });

            services.AddAuthorization(options=>{
                options.AddPolicy("RequireCustomerLoggedIn",policy=>policy.RequireRole("ITAdmin","Customer","CustomerService"));
                options.AddPolicy("RequireLoggedIn",policy=> policy.RequireRole("ITAdmin","Customer","OperationManager","Accounts","DeliveryManager","Kitchen","DeliveryAgents","CustomerService").RequireAuthenticatedUser());//.RequireClaim("abc"));
                options.AddPolicy("RequireAdministratorRole",policy=>policy.RequireRole("ITAdmin").RequireAuthenticatedUser());    
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("EnableCORS");
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
