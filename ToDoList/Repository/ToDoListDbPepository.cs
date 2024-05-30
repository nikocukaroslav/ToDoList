using Dapper;
using ToDoList.Data;
using ToDoList.Models.Domain;

namespace ToDoList.Repository;

public class ToDoListDbPepository : IToDoListRepository
{
    private readonly ToDoListDbContext _toDoListDbContext;

    public ToDoListDbPepository(ToDoListDbContext toDoListDbContext)
    {
        _toDoListDbContext = toDoListDbContext;
    }

    public async Task<List<ToDo>> GetAllToDos()
    {
        var connection = _toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM ToDo";
        
        var todos = await connection.QueryAsync<ToDo>(sql);

        return todos.ToList();
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var connection = _toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM Category";

        var categories =  await connection.QueryAsync<Category>(sql);

        return categories.ToList();
    }

    public async Task AddCategory(Category category)
    {
        var connection = _toDoListDbContext.CreateConnection();

        var newCategory = new Category
        {
            Id = category.Id,
            Name = category.Name,
        };
        
        var sql = "INSERT INTO Category (Id, Name) VALUES (@Id, @Name)";

        await connection.ExecuteAsync(sql, newCategory);
    }
    
    public async Task AddToDo(ToDo todo)
    {
        var connection = _toDoListDbContext.CreateConnection();

        var newTodo = new ToDo
        {
            Id = todo.Id,
            Task = todo.Task,
            CategoryName = todo.CategoryName,
            DateToPerform = todo.DateToPerform,
        };
        
        var sql = "INSERT INTO ToDo (Id, Task, IsPerformed, DateToPerform, CategoryName) " +
                  "VALUES (@Id, @Task, @IsPerformed, @DateToPerform, @CategoryName)";

        await connection.ExecuteAsync(sql, newTodo);
    }

    public async Task HandlePerformed(ToDo todo)
    {
        var connection = _toDoListDbContext.CreateConnection();
        
        var performToDo = new ToDo
        {
            Id = todo.Id,
            IsPerformed = !todo.IsPerformed,
        };

        var sql = "UPDATE ToDo SET IsPerformed = @IsPerformed WHERE Id = @Id";

        await connection.ExecuteAsync(sql, performToDo);
    }

    public async Task DeleteToDo(ToDo todo)
    {
        var connection = _toDoListDbContext.CreateConnection();

        var todoToDelete = new ToDo
        {
            Id = todo.Id,
        };
        
        var sql = "DELETE FROM ToDo WHERE Id = @Id";
        
        await connection.ExecuteAsync(sql, todoToDelete);
    }
}