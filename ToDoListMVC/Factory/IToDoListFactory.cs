using ToDoList.Data;
using ToDoList.Repository;

namespace ToDoList.Factory;

public interface IToDoListFactory
{
    IToDoListRepository? GetToDoListRepository();
}