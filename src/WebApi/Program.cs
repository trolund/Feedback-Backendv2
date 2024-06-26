﻿using System;
using Data.Contexts;
using Data.Contexts.Seeding;
using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi {
    public class Program {
        public static void Main (string[] args) {
            var host = CreateHostBuilder (args)
                .Build ();

            // database seeding
            using (var scope = host.Services.CreateScope ()) {
                var services = scope.ServiceProvider;
                var context = scope.ServiceProvider.GetService<ApplicationDbContext> ();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>> ();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>> ();
                DBSeeding.Seed (context, userManager, roleManager).Wait ();
            }

            host.Run ();
        }

        public static IHostBuilder CreateHostBuilder (string[] args) =>
            Host.CreateDefaultBuilder (args)
            .ConfigureWebHostDefaults (webBuilder => {
                // use https in production og http on local dev mashine
                var env = Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT");
                if (env == "Development") {
                    webBuilder.UseStartup<Startup> ()
                        .UseUrls ("https://*:4000");
                } else {
                    webBuilder.UseStartup<Startup> ()
                        .UseUrls ("http://*:4000");
                }
            });
    }
}