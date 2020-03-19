using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddLogging ();

            // add services
            services.AddScoped<IMeetingService, MeetingService> ();
            services.AddScoped<IFeedbackService, FeedbackService> ();
            services.AddScoped<IFeedbackBatchService, FeedbackBatchService> ();
            services.AddScoped<IQuestionSetService, QuestionSetService> ();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();

            services.AddAutoMapper (typeof (Startup));

            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (
                    Configuration.GetConnectionString ("DefaultConnection")));

            services.AddSwaggerGen (options => {
                options.SwaggerDoc ("v1", new OpenApiInfo { Title = "Feedback API", Version = "v0.0.1" });

                options.AddSecurityDefinition ("Bearer", new OpenApiSecurityScheme {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"

                });

                options.AddSecurityRequirement (new OpenApiSecurityRequirement () {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                        },
                        new List<string> ()
                    }
                });
            });

            services.AddCors ();
            services.AddControllers ();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection ("AppSettings");
            services.Configure<AppSettings> (appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings> ();
            var key = Encoding.ASCII.GetBytes (appSettings.Secret);
            services.AddAuthentication (x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // configure DI for application services
            services.AddScoped<IUserService, UserService> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services) {

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                //app.UseDatabaseErrorPage();

                app.UseSwagger (options => {
                    options.RouteTemplate = "/api/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI (options => {
                    options.SwaggerEndpoint ("/api/swagger/v1/swagger.json", "Feedback API Gateway v1");
                    options.RoutePrefix = "api/swagger";
                });
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseRouting ();

            // global cors policy
            app.UseCors (x => x
                .AllowAnyOrigin ()
                .AllowAnyMethod ()
                .AllowAnyHeader ());

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });

            //CreateRoles(services).Wait();
        }

        private string[] roles = { Roles.ADMIN, Roles.VADMIN, Roles.FACILITATOR };

        private async Task CreateRoles (IServiceProvider serviceProvider) {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>> ();
            IdentityResult roleResult;

            foreach (string role in roles) {
                //here in this line we are adding Admin Role
                var roleCheck = await RoleManager.RoleExistsAsync (role);
                if (!roleCheck) {
                    //here in this line we are creating admin role and seed it to the database
                    roleResult = await RoleManager.CreateAsync (new IdentityRole (role));
                }
            }

            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>> ();
            ApplicationUser user = await UserManager.FindByEmailAsync ("trolund@gmail.com");

            if (user != null) {
                await UserManager.AddToRoleAsync (user, Roles.ADMIN);
                await UserManager.AddToRoleAsync (user, Roles.VADMIN);
            }

        }
    }
}