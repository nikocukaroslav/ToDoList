using GraphQL.Types;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Query;

public sealed class RootQuery : ObjectGraphType
{
    public RootQuery(IToDoListRepository toDoListRepository)
    {

        Field<ListGraphType<ToDoType>>("todos").Resolve(context =>
            toDoListRepository.GetAllToDos()
        );

        Field<ListGraphType<CategoryType>>("categories").Resolve(context =>
             toDoListRepository.GetAllCategories()
        );
    }
}