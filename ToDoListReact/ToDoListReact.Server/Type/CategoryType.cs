using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Factory;
using ToDoList.Models.Domain;
using ToDoList.Repository;

namespace ToDoListAPI.Type;

public sealed class CategoryType : ObjectGraphType<Category>
{
    public CategoryType(StorageChanger storageChanger)
    {
        Field(x => x.Id);
        Field(x => x.Name);
        Field<ListGraphType<ToDoType>>("ToDos").Resolve(context =>
        {
            var todoListRepository = storageChanger.GetToDoListRepository();

            return todoListRepository.GetAllToDos();
        });

    }
}