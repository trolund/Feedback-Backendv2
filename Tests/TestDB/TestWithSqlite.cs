using System;
using Data.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public abstract class TestWithSqlite : IDisposable {
    private const string InMemoryConnectionString = "DataSource=file::memory:?cache=shared";
    private readonly SqliteConnection _connection;

    protected readonly ApplicationDbContext DbContext;

    protected TestWithSqlite () {
        _connection = new SqliteConnection (InMemoryConnectionString);
        _connection.Open ();
        var options = new DbContextOptionsBuilder<ApplicationDbContext> ()
            .UseSqlite (_connection)
            .Options;
        DbContext = new ApplicationDbContext (options, null);
        DbContext.Database.EnsureCreated ();
    }

    public void Dispose () {
        _connection.Close ();
    }
}