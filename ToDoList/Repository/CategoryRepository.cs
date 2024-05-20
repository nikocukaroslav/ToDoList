using Dapper;
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

    public async Task Add(Category category)
    {
        var connection = toDoListDbContext.CreateConnection();

        var sql = "INSERT INTO (Id Name) VALUES (@Id @Name)";

        await connection.ExecuteAsync(sql, category);
    }
}