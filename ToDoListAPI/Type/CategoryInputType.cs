using GraphQL.Types;

namespace ToDoListAPI.Type;

public sealed class CategoryInputType : InputObjectGraphType
{
    public CategoryInputType()
    {
        Field<GuidGraphType>("id");

        Field<StringGraphType>("name");
    }
}