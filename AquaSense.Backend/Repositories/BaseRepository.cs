using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AquaSense.Backend.Repositories;

public abstract class BaseRepository
{
    protected BaseRepository(IConfiguration config, ILogger logger)
    {
        Logger = logger;
        var connectionString = config.GetConnectionString("Supabase")
            ?? throw new InvalidOperationException("Connection string 'Supabase' is missing.");
        DataSource = NpgsqlDataSource.Create(connectionString);
    }

    protected NpgsqlDataSource DataSource { get; }
    protected ILogger Logger { get; }

    protected NpgsqlCommand CreateCommand(string sql)
    {
        return DataSource.CreateCommand(sql);
    }
}
