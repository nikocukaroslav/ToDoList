using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public class ToDoListDbContext
{
    private readonly IConfiguration configuration;
    private readonly string? connectionString;

    public ToDoListDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
        connectionString = this.configuration.GetConnectionString("ToDoListDbConnectionString");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}