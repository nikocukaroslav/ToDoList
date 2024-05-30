using GraphQL.Types;

namespace ToDoListAPI.Mutation;

public sealed class RootMutation : ObjectGraphType
{
    public RootMutation()
    {
        Field<ToDoMutation>("todoMutation").Resolve(context => new { });
        Field<CategoryMutation>("categoryMutation").Resolve(context => new { });
    }
}