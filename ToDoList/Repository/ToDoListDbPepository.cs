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

    public List<ToDo> GetAllToDos()
    {
        var connection = _toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM ToDo";
        
        var todos =  connection.Query<ToDo>(sql);

        return todos.ToList();
    }

    public List<Category> GetAllCategories()
    {
        var connection = _toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM Category";

        var categories = connection.Query<Category>(sql);

        return categories.ToList();
    }

    public Category AddCategory(Category category)
    {
        var connection = _toDoListDbContext.CreateConnection();

        var newCategory = new Category
        {
            Id = category.Id,
            Name = category.Name,
        };
        
        var sql = "INSERT INTO Category (Id, Name) VALUES (@Id, @Name)";

         connection.Execute(sql, newCategory);

         return newCategory;
    }
    
    public ToDo AddToDo(ToDo todo)
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

       connection.Execute(sql, newTodo);
       
       return newTodo;
    }

    public ToDo HandlePerformed(ToDo todo)
    {
        var connection = _toDoListDbContext.CreateConnection();
        
        var performToDo = new ToDo
        {
            Id = todo.Id,
            IsPerformed = !todo.IsPerformed,
        };

        var sql = "UPDATE ToDo SET IsPerformed = @IsPerformed WHERE Id = @Id";

        connection.Execute(sql, performToDo);

        return performToDo;
    }

    public void DeleteToDo(ToDo todo)
    {
        var connection = _toDoListDbContext.CreateConnection();

        var todoToDelete = new ToDo
        {
            Id = todo.Id,
        };
        
        var sql = "DELETE FROM ToDo WHERE Id = @Id";
        
         connection.Execute(sql, todoToDelete);
    }
}