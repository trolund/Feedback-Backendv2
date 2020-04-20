﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Helpers;
using Business.Services;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApi {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddLogging ();

            services.AddScoped<IUnitOfWork, UnitOfWork> ();

            // add services
            // configure DI for application services
            services.AddScoped<IUserService, UserService> ();
            services.AddTransient<IMeetingService, MeetingService> ();
            services.AddTransient<IFeedbackService, FeedbackService> ();
            services.AddTransient<IFeedbackBatchService, FeedbackBatchService> ();
            services.AddTransient<IQuestionSetService, QuestionSetService> ();
            services.AddTransient<IUserService, UserService> ();
            services.AddTransient<ICompanyService, CompanyService> ();

            services.AddTransient<IEmailService, EmailService> ();

            //  services.AddScoped<IRepository, Repository> ();
            services.AddScoped<ICategoryRepository, CategoryRepository> ();
            services.AddScoped<ICompanyRepository, CompanyRepository> ();
            services.AddScoped<IFeedbackBatchRepository, FeedbackBatchRepository> ();
            services.AddScoped<IFeedbackRepository, FeedbackRepository> ();
            services.AddScoped<IMeetingCategoryRepository, MeetingCategoryRepository> ();
            services.AddScoped<IMeetingRepository, MeetingRepository> ();
            services.AddScoped<IQuestionSetRepository, QuestionSetRepository> ();
            services.AddScoped<IUserRepository, UserRepository> ();

            // services.AddSingleton<IHostedService, ScheduleTask> ();

            services.AddHttpContextAccessor ();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor> ();
            services.AddScoped<IUrlHelper> (x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor> ().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory> ();
                return factory.GetUrlHelper (actionContext);
            });

            services.AddAutoMapper (typeof (Startup));

            services.AddIdentity<ApplicationUser, IdentityRole> (options => {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext> ()
                .AddDefaultTokenProviders ();

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
            services.AddControllers ().AddNewtonsoftJson ();

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

        }
    }
}