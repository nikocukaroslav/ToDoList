using ToDoList.Data;

namespace ToDoList.Factory;

public interface IToDoListFactory
{
    IToDoListRepository? GetToDoListRepository();
}