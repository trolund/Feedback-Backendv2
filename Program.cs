using Data.Contexts;
using Data.Contexts.Seeding;
using Microsoft.AspNetCore.Hosting;
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
                DBSeeding.Seed (context);
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