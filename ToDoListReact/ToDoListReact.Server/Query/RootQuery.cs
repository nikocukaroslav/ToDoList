using GraphQL.Types;
using ToDoList.Factory;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Query;

public sealed class RootQuery : ObjectGraphType
{
    public RootQuery(StorageChanger storageChanger)
    {

        Field<ListGraphType<ToDoType>>("todos").Resolve(context =>
        {
            var todoListRepository = storageChanger.GetToDoListRepository();

            return todoListRepository.GetAllToDos();
        }); 

        Field<ListGraphType<CategoryType>>("categories").Resolve(context =>
        {

            var todoListRepository = storageChanger.GetToDoListRepository();

            return todoListRepository.GetAllCategories();
        });
    }
}