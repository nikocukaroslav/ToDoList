using ToDoList.Models.Domain;

namespace ToDoList.Repository;

public interface IToDoListRepository
{
    Task<List<ToDo>> GetAllToDos();
    Task<List<Category>> GetAllCategories();
    Task AddCategory(Category category);
    Task AddToDo(ToDo todo);
    Task HandlePerformed(ToDo todo);
    Task DeleteToDo(ToDo todo);

}