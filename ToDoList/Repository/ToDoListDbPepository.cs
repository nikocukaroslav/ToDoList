using Dapper;
using ToDoList.Models;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public class ToDoListDbPepository : IToDoListRepository
{
    private readonly ToDoListDbContext toDoListDbContext;

    public ToDoListDbPepository(ToDoListDbContext toDoListDbContext)
    {
        this.toDoListDbContext = toDoListDbContext;
    }

    public async Task<List<ToDo>> GetAllToDos()
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM ToDo";
        
        var todos = await connection.QueryAsync<ToDo>(sql);

        return todos.ToList();
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM Category";

        var categories =  await connection.QueryAsync<Category>(sql);

        return categories.ToList();
    }

    public async Task AddCategory(AddCategoryRequest addCategoryRequest)
    {
        var connection = toDoListDbContext.CreateConnection();

        var category = new Category
        {
            Id = addCategoryRequest.Id,
            Name = addCategoryRequest.Name,
        };
        
        var sql = "INSERT INTO Category (Id, Name) VALUES (@Id, @Name)";

        await connection.ExecuteAsync(sql, category);
    }
    
    public async Task AddToDo(AddToDoRequest addToDoRequest)
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
    

    public async Task DeleteToDo(DeleteToDoRequest deleteToDoRequest)
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