using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using timetracker.Models;
using timetracker.Interfaces;
using timetracker.Repositories;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.AspNetCore.Authorization;

namespace timetracker
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
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = signingKey,
               ValidateIssuer = true,
               ValidIssuer = issuer,
               ValidateAudience = true,
               ValidAudience = audience,
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddAuthorization(options => 
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });
            var connection = Configuration["SqliteConnectionString"];

            services.AddMvc();
            services.AddDbContextPool<TimeTrackerDbContext>(options => options.UseSqlite(connection));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<TimeTrackerDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
