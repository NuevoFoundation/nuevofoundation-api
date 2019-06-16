using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataAccess.Interfaces;
using API.Models;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API
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
      InitializeSettings();

      #if DEBUG
      services.AddCors(options =>
      {
        options.AddPolicy("AllowSpecificOrigin",
          builder => builder.WithOrigins(new string[] {
            "http://localhost:3000"}).AllowAnyHeader().AllowAnyMethod());
      });
      #endif

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Settings.JwtIssuer,
          ValidAudience = Settings.JwtAudience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JwtSecretKey))
        };
      });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddSingleton<IStorage<VirtualSession>>(new DocumentDB<VirtualSession>(Settings.StorageEndpoint, Settings.StorageKey, Settings.StorageDatabaseId, "virtualsessions"));
      services.AddSingleton<IStorage<Member>>(new DocumentDB<Member>(Settings.StorageEndpoint, Settings.StorageKey, Settings.StorageDatabaseId, "members"));
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
        app.UseHttpsRedirection();
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      #if DEBUG
      app.UseCors("AllowSpecificOrigin");
      #endif
      app.UseAuthentication();
      app.UseMvc();
    }

    public void InitializeSettings()
    {
      Settings.StorageEndpoint = Configuration["StorageEndpoint"];
      Settings.StorageKey = Configuration["StorageKey"];
      Settings.StorageDatabaseId = Configuration["StorageDatabaseId"];
      Settings.ClientId = Configuration["ClientId"];
      Settings.ClientSecret = Configuration["ClientSecret"];
      Settings.Scope = Configuration["Scope"];
      Settings.Email = Configuration["Email"];
      Settings.EmailPassword = Configuration["EmailPassword"];
      Settings.TenantId = Configuration["TenantId"];
      Settings.FrontendUrl = Configuration["FrontendUrl"];
      Settings.JwtIssuer = Configuration["JwtIssuer"];
      Settings.JwtAudience = Configuration["JwtAudience"];
      Settings.JwtSecretKey = Configuration["JwtSecretKey"];
    }
  }
}
