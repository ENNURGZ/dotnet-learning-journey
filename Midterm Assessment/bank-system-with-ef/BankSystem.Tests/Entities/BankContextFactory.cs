using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection;
using System.Resources;
using BankSystem.EF.Entities;
using BankSystem.Tests.AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Tests.Entities;

public sealed class BankContextFactory : IDisposable
{
    private readonly string databaseFile = FileIOHelper.GetDBFullPath("bank-system.db");
    private readonly SqliteConnection connection;

    public BankContextFactory()
    {
        this.connection = this.CreateConnection();
    }

    public BankContextFactory(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public BankContext CreateContext()
    {
        return new BankContext(this.CreateOptions());
    }

    public DbContextOptions<BankContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<BankContext>()
            .UseSqlite(this.connection)
            .Options;
    }

    public IReadOnlyList<T> GetEntitiesBySqlQueryName<T>(string sqlQueryName, Func<DbDataReader, T> builder)
    {
        const string resourceManifestName = "BankSystem.Tests.Properties.Resources.resources";

        var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceManifestName);
        var set = new ResourceSet(resourceStream!);
        var sqlQuery = set.GetString(sqlQueryName);
        set.Dispose();
        return this.GetEntitiesBySqlQuery(sqlQuery, builder);
    }

    public IReadOnlyList<T> GetEntitiesBySqlQuery<T>(string? commandText, Func<DbDataReader, T> builder)
    {
        var list = new List<T>();

        using (var command = new SqliteCommand(commandText, this.connection))
        {
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var entity = builder(reader);
                list.Add(entity);
            }
        }

        return new ReadOnlyCollection<T>(list);
    }

    public void Dispose()
    {
        this.connection.Dispose();
    }

    private SqliteConnection CreateConnection()
    {
        var connectionString = FileConnectionString(this.databaseFile, readOnly: true);
        var connectn = new SqliteConnection(connectionString);
        connectn.Open();
        return connectn;
    }

    private static string FileConnectionString(string fileName, bool readOnly = false) => new SqliteConnectionStringBuilder
    {
        DataSource = fileName,
        Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate,
    }.ConnectionString;
}
