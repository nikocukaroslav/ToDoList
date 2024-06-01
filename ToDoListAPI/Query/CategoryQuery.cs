using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Query;

public sealed class CategoryQuery : ObjectGraphType
{
    public CategoryQuery(IToDoListRepository toDoListRepository)
    {
        Field<ListGraphType<CategoryType>>("categories").Resolve( context =>
             toDoListRepository.GetAllCategories()
        );
    }
}