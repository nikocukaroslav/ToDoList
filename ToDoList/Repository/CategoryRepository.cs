using Dapper;
using ToDoList.Models;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public class CategoryRepository : ICategoryRepository
{
    private readonly ToDoListDbContext toDoListDbContext;

    public CategoryRepository(ToDoListDbContext toDoListDbContext)
    {
        this.toDoListDbContext = toDoListDbContext;
    }

    public async Task<List<Category>> GetAll()
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "SELECT * FROM Category";

        var categories =  await connection.QueryAsync<Category>(sql);

        return categories.ToList();
    }

    public async Task Add(AddCategoryRequest addCategoryRequest)
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
}