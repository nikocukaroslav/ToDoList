using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Models.Domain;

namespace ToDoListAPI.Type;

public sealed class CategoryType : ObjectGraphType<Category>
{
   public CategoryType(IToDoListRepository todoListRepository)
   {
      Field(x => x.Id);
      Field(x => x.Name);
      Field<ListGraphType<ToDoType>>("ToDos").ResolveAsync(async context => await todoListRepository.GetAllToDos());
   }
}