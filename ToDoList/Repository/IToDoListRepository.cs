using ToDoList.Models;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface IToDoListRepository
{
    Task<List<ToDo>> GetAllToDos();
    Task<List<Category>> GetAllCategories();
    Task AddCategory(AddCategoryRequest addCategoryRequest);
    Task AddToDo(AddToDoRequest addToDoRequest);
    Task PerformToDo(HandleTodoRequest handleTodoRequest);
    Task UnperformToDo(HandleTodoRequest handleTodoRequest);
    Task DeleteToDo(DeleteToDoRequest deleteToDoRequest);

}