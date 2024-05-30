using GraphQL;
using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Models.Domain;
using ToDoListAPI.Type;

namespace ToDoListAPI.Mutation;

public sealed class ToDoMutation : ObjectGraphType
{
    public ToDoMutation(IToDoListRepository todoListRepository)
    {
        Field<ToDoType>("addToDo").Arguments(new QueryArguments(new QueryArgument<ToDoInputType>
        {
            Name = "addToDo"
        }
        )).ResolveAsync(async context =>
        {
            return  todoListRepository.AddToDo(context.GetArgument<ToDo>("addToDo"));
        });

        Field<ToDoType>("handlePerformed").Arguments(new QueryArguments(new QueryArgument<ToDoInputType>
        {
            Name = "handlePerformed"
        }
        )).ResolveAsync(async context =>
        {
            return todoListRepository.HandlePerformed(context.GetArgument<ToDo>("handlePerformed"));
        });

        Field<GuidGraphType>("deleteToDo").Arguments(new QueryArguments(new QueryArgument<GuidGraphType>
        {
            Name = "id"
        }
        )).Resolve(context =>
        {
            ToDo todoId = context.GetArgument<ToDo>("id");

            todoListRepository.DeleteToDo(todoId);

            return "The todo against this Id " + todoId + " has been deleted";
        });
    }
}