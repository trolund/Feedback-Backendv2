using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests {
    public class DbSetup {

        public ServiceProvider ServiceProvider { get; private set; }
        public DbSetup () {
            var serviceCollection = new ServiceCollection ();
            serviceCollection
                .AddDbContext<ApplicationDbContext> (options => options.UseInMemoryDatabase (databaseName: "Add_writes_to_database"),
                    ServiceLifetime.Transient);

            ServiceProvider = serviceCollection.BuildServiceProvider ();
        }

    }
}