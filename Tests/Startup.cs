using System;
using System.Reflection;
using AutoMapper;
using Business.Services;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Seeding;
using Data.Contexts_access;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

[assembly : TestFramework ("Tests.Startup", "Tests")]

namespace Tests {
    public class Startup : DependencyInjectionTestFramework {
        public Startup (IMessageSink messageSink) : base (messageSink) { }

        protected void ConfigureServices (IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork> ();

            // add services
            services.AddTransient<IMeetingService, MeetingService> ();
            services.AddTransient<IFeedbackService, FeedbackService> ();
            services.AddTransient<IFeedbackBatchService, FeedbackBatchService> ();
            services.AddTransient<IQuestionSetService, QuestionSetService> ();
            services.AddTransient<IUserService, UserService> ();
            services.AddTransient<ICompanyService, CompanyService> ();

            services.AddTransient<IEmailService, EmailService> ();

            services.AddTransient<ICategoryRepository, CategoryRepository> ();
            services.AddTransient<ICompanyRepository, CompanyRepository> ();
            services.AddTransient<IFeedbackBatchRepository, FeedbackBatchRepository> ();
            services.AddTransient<IFeedbackRepository, FeedbackRepository> ();
            services.AddTransient<IMeetingCategoryRepository, MeetingCategoryRepository> ();
            services.AddTransient<IMeetingRepository, MeetingRepository> ();
            services.AddTransient<IQuestionSetRepository, QuestionSetRepository> ();
            services.AddTransient<IUserRepository, UserRepository> ();

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
                options.UseSqlite ("DataSource=file::memory:?cache=shared")
            );

            services.AddHttpContextAccessor ();

            // use automapping profiles find i data project
            services.AddAutoMapper (typeof (Profiles));

        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services) {
            // var context = services.GetService<ApplicationDbContext> ();
            // var userManager = services.GetService<UserManager<ApplicationUser>> ();
            // var roleManager = services.GetRequiredService<RoleManager<IdentityRole>> ();
            // Console.WriteLine ("seeding db");
            // context.Database.OpenConnection ();
            // context.Database.Migrate ();
            // DBSeeding.Seed (context, userManager, roleManager).Wait ();
        }

        protected override IHostBuilder CreateHostBuilder (AssemblyName assemblyName) =>
            base.CreateHostBuilder (assemblyName)
            .ConfigureServices (ConfigureServices);

    }
}