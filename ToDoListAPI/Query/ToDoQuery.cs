using GraphQL.Types;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Query;

public sealed class ToDoQuery : ObjectGraphType
{
    public ToDoQuery(IToDoListRepository toDoListRepository)
    {
        Field<ListGraphType<ToDoType>>("todos").Resolve(context =>
             toDoListRepository.GetAllToDos()
        );
    }
}