using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface IToDoRepository
{
    Task<List<ToDo>> GetAll();
    Task Add(ToDo todo);
    Task ChangePerformed(ToDo todo);
    Task Delete(ToDo todo);

}