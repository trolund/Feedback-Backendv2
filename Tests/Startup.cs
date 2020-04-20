using System.Reflection;
using AutoMapper;
using Business.Services;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interface;
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

            //  services.AddScoped<IRepository, Repository> ();
            services.AddScoped<ICategoryRepository, CategoryRepository> ();
            services.AddScoped<ICompanyRepository, CompanyRepository> ();
            services.AddScoped<IFeedbackBatchRepository, FeedbackBatchRepository> ();
            services.AddScoped<IFeedbackRepository, FeedbackRepository> ();
            services.AddScoped<IMeetingCategoryRepository, MeetingCategoryRepository> ();
            services.AddScoped<IMeetingRepository, MeetingRepository> ();
            services.AddScoped<IQuestionSetRepository, QuestionSetRepository> ();
            services.AddScoped<IUserRepository, UserRepository> ();

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
                options.UseInMemoryDatabase ("TestDB")
            );

            services.AddHttpContextAccessor ();
            services.AddAutoMapper (typeof (Startup));
        }

        protected override IHostBuilder CreateHostBuilder (AssemblyName assemblyName) =>
            base.CreateHostBuilder (assemblyName)
            .ConfigureServices (ConfigureServices);
    }
}