using Dapper;
using ToDoList.Models;
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

    public async Task Add(AddToDoRequest addToDoRequest)
    {
        var connection = toDoListDbContext.CreateConnection();

        var todo = new ToDo
        {
            Id = addToDoRequest.Id,
            Task = addToDoRequest.Task,
            CategoryName = addToDoRequest.CategoryName,
            DateToPerform = addToDoRequest.DateToPerform,
        };
        
        var sql = "INSERT INTO ToDo (Id, Task, IsPerformed, DateToPerform, CategoryName) " +
                  "VALUES (@Id, @Task, @IsPerformed, @DateToPerform, @CategoryName)";

        await connection.ExecuteAsync(sql, todo);
    }

    public async Task PerformToDo(HandleTodoRequest handleTodoRequest)
    {
        var connection = toDoListDbContext.CreateConnection();
        
        var perfromToDo = new ToDo
        {
            Id = handleTodoRequest.Id,
            IsPerformed = true,
        };

        var sql = "UPDATE ToDo SET IsPerformed = @IsPerformed WHERE Id = @Id";

        await connection.ExecuteAsync(sql, perfromToDo);
    }
    
    public async Task UnperformToDo(HandleTodoRequest handleTodoRequest)
    {
        var connection = toDoListDbContext.CreateConnection();
        
        var perfromToDo = new ToDo
        {
            Id = handleTodoRequest.Id,
            IsPerformed = false,
        };
        
        var sql = "UPDATE ToDo SET IsPerformed = @IsPerformed WHERE Id = @Id";

        await connection.ExecuteAsync(sql, perfromToDo);
    }
    

    public async Task Delete(DeleteToDoRequest deleteToDoRequest)
    {
        var connection = toDoListDbContext.CreateConnection();

        var todoToDelete = new ToDo
        {
            Id = deleteToDoRequest.Id,
        };
        
        var sql = "DELETE FROM ToDo WHERE Id = @Id";
        
        await connection.ExecuteAsync(sql, todoToDelete);
    }
}