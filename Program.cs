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
            using (var scope = host.Services.CreateScope ()) {
                var services = scope.ServiceProvider;
                var context = scope.ServiceProvider.GetService<ApplicationDbContext> ();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>> ();
                DBSeeding.Seed (context, userManager);
            }
            host.Run ();
        }

        public static IHostBuilder CreateHostBuilder (string[] args) =>
            Host.CreateDefaultBuilder (args)
            .ConfigureWebHostDefaults (webBuilder => {
                webBuilder.UseStartup<Startup> ()
                    .UseUrls ("http://localhost:4000");
            });
    }
}