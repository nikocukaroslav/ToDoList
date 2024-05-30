using GraphQL.Types;

namespace ToDoListAPI.Type;

public sealed class ToDoInputType : InputObjectGraphType
{
    public ToDoInputType()
    {
        Field<GuidGraphType>("id");
        Field<StringGraphType>("task");
        Field<BooleanGraphType>("isPerformed");
        Field<StringGraphType>("categoryName");
        Field<DateGraphType>("dateToPerform");
    }
}