using System;
using Data.Contexts;
using Data.Contexts.Seeding;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests {
    public class UnitTest1 {

        private readonly IUnitOfWork _UnitOfWork;
        public UnitTest1 (IUnitOfWork u) {
            _UnitOfWork = u;
        }

        [Fact]
        public void Test1 () {

        }

        [Fact]
        public void Test2 () {

        }
    }
}