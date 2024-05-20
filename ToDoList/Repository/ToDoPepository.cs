using Dapper;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public class ToDoPepository : IToDoRepository
{
    private readonly ToDoListDbContext toDoListDbContext;

    public ToDoPepository(ToDoListDbContext toDoListDbContext)
    {
        this.toDoListDbContext = toDoListDbContext;
    }

    public async Task<List<ToDo>> GetAll()
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM ToDo";
        
        var todos = await connection.QueryAsync<ToDo>(sql);

        return todos.ToList();
    }

    public async Task Add(ToDo todo)
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "INSERT INTO ToDo (Id, Task, IsPerformed, DateToPerform, CategoryName) " +
                  "VALUES (@Id, @Task, @IsPerformed, @DateToPerform, @CategoryName)";

        await connection.ExecuteAsync(sql, todo);
    }

    public async Task ChangePerformed(ToDo todo)
    {
        var connection = toDoListDbContext.CreateConnection();
        
        var sql = "UPDATE ToDo SET IsPerformed = @IsPerformed WHERE Id = @Id";

        await connection.ExecuteAsync(sql, todo);
    }
    

    public async Task Delete(ToDo todo)
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "DELETE FROM ToDo WHERE Id = @Id";
        
        await connection.ExecuteAsync(sql, todo);
    }
}