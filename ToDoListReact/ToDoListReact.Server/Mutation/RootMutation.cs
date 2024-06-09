using GraphQL;
using GraphQL.Types;
using ToDoList.Factory;
using ToDoList.Models.Domain;
using ToDoList.Repository;
using ToDoListAPI.Type;

namespace ToDoListAPI.Mutation;

public sealed class RootMutation : ObjectGraphType
{
    public RootMutation(StorageChanger storageChanger)
    {

        Field<ToDoType>("addToDo").Arguments(new QueryArguments(new QueryArgument<ToDoInputType>
        {
            Name = "todo"
        }
        )).Resolve(context =>
        {

            var todoListRepository = storageChanger.GetToDoListRepository();

            var todo = context.GetArgument<ToDo>("todo");

            return todoListRepository.AddToDo(todo);
        }
        );

        Field<ToDoType>("handlePerformed").Arguments(new QueryArguments(new QueryArgument<ToDoInputType>
        {
            Name = "handlePerformed"
        }
        )).Resolve(context =>
        {
            var todoListRepository = storageChanger.GetToDoListRepository();

            var handledToDo = context.GetArgument<ToDo>("handlePerformed");

            return todoListRepository.HandlePerformed(handledToDo);
        });

        Field<StringGraphType>("deleteToDo").Arguments(new QueryArguments(new QueryArgument<IdGraphType>
        {
            Name = "id"
        }
        )).Resolve(context =>
        {
            var todoId = context.GetArgument<Guid>("id");
            var todoToDelete = new ToDo
            {
                Id = todoId,
            };

            var todoListRepository = storageChanger.GetToDoListRepository();

            todoListRepository.DeleteToDo(todoToDelete);

            return "ToDo with id " + todoId + " has been deleted";
        });

        Field<CategoryType>("addCategory").Arguments(new QueryArguments(new QueryArgument<CategoryInputType>
        {
            Name = "category"
        }
           )).Resolve(context =>
           {
               var todoListRepository = storageChanger.GetToDoListRepository();

               return todoListRepository.AddCategory(context.GetArgument<Category>("category"));
           });
    }
}