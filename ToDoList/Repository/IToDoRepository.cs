using ToDoList.Models;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface IToDoRepository
{
    Task<List<ToDo>> GetAll();
    Task Add(AddToDoRequest addToDoRequest);
    Task PerformToDo(HandleTodoRequest handleTodoRequest);
    Task UnperformToDo(HandleTodoRequest handleTodoRequest);
    Task Delete(DeleteToDoRequest deleteToDoRequest);

}