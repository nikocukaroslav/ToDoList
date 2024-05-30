using GraphiQl;
using GraphQL;
using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Repository;
using ToDoListAPI.Mutation;
using ToDoListAPI.Query;
using ToDoListAPI.Schema;
using ToDoListAPI.Type;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ToDoListDbContext>();
builder.Services.AddTransient<IToDoListRepository, ToDoListDbPepository>();

builder.Services.AddTransient<CategoryType>();
builder.Services.AddTransient<ToDoType>();
builder.Services.AddTransient<CategoryInputType>();
builder.Services.AddTransient<ToDoInputType>();

builder.Services.AddTransient<CategoryQuery>();
builder.Services.AddTransient<ToDoQuery>();
builder.Services.AddTransient<RootQuery>();

builder.Services.AddTransient<ToDoMutation>();
builder.Services.AddTransient<CategoryMutation>();
builder.Services.AddTransient<RootMutation>();

builder.Services.AddTransient<ISchema,RootSchema>();

builder.Services.AddGraphQL(options =>
{
    options.AddAutoSchema<ISchema>().AddSystemTextJson();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGraphiQl("/graphql");

app.UseGraphQL<ISchema>();

app.UseHttpsRedirection();

app.Run();