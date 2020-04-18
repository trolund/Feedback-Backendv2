using Data.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Tests;
using Xunit;

namespace Test {

    public class UnitTest1 : IClassFixture<DbSetup> {
        private ServiceProvider _serviceProvide;

        public UnitTest1 (DbSetup fixture) {
            // _serviceProvide = fixture.ServiceProvider;
        }

        [Fact]
        public void Test1 () {
            // using (var context = _serviceProvide.GetService<ApplicationDbContext> ()) { }
            Assert.False (false, "1 should not be prime");
        }

        [Fact]
        public void IsPrime_InputIs1_ReturnFalse () {
            var result = false;

            Assert.False (result, "1 should not be prime");
        }
    }
}