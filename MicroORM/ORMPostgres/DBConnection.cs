using Npgsql;

namespace ORM.ORMPostgres;

class DBConnection : IDisposable
{
    string connectionString;
    NpgsqlConnection connection;

    public DBConnection(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public NpgsqlConnection GetConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        return connection;
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}
