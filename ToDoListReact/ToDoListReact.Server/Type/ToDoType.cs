using GraphQL.Types;
using ToDoList.Models.Domain;

namespace ToDoListAPI.Type;

public sealed class ToDoType : ObjectGraphType<ToDo>
{
    public ToDoType()
    {
        Field(x => x.Id);
        Field(x => x.Task);
        Field(x => x.IsPerformed);
        Field(x => x.CategoryName);
        Field(x => x.DateToPerform, nullable: true);
    }
}