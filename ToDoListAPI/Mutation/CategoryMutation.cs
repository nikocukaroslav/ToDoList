using GraphQL;
using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Models.Domain;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Mutation
{
    public sealed class CategoryMutation : ObjectGraphType
    {
        public CategoryMutation(IToDoListRepository todoListRepository)
        {
            Field<CategoryType>("addCategory").Arguments(new QueryArguments(new QueryArgument<CategoryInputType>
            {
                Name = "category"
            }
            )).Resolve(context =>
            {
                  return todoListRepository.AddCategory(context.GetArgument<Category>("category"));
            });
        }
    }
}
