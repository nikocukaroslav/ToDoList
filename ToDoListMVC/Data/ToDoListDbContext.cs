using System.Data;
using Microsoft.Data.SqlClient;

namespace ToDoList.Data;

public class ToDoListDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public ToDoListDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("ToDoListDbConnectionString");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}