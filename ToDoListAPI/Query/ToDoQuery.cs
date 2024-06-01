using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Query;

public sealed class ToDoQuery : ObjectGraphType
{
    public ToDoQuery(IToDoListRepository toDoListRepository)
    {
        Field<ListGraphType<ToDoType>>("todos").ResolveAsync(async context =>
            await toDoListRepository.GetAllToDos()
        );
    }
}