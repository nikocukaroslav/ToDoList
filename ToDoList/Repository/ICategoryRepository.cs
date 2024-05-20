using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface ICategoryRepository
{
    Task<List<Category>> GetAll();
    Task Add(Category category);
}