using GraphQL.Types;

namespace ToDoListAPI.Query;

public sealed class RootQuery : ObjectGraphType
{
    public RootQuery()
    {
        Field<ToDoQuery>("todoQuery").Resolve(context => new { });
        Field<CategoryQuery>("categoryQuery").Resolve(context => new { });
    }
}