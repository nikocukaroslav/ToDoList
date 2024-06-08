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

builder.Services.AddTransient<RootQuery>();

builder.Services.AddTransient<RootMutation>();

builder.Services.AddTransient<ISchema, RootSchema>();

builder.Services.AddGraphQL(options =>
{
    options.AddAutoSchema<ISchema>().AddSystemTextJson();
});

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("React_Url");

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGraphiQl("/graphql");

app.UseCors();

app.UseGraphQL<ISchema>();

app.UseHttpsRedirection();

app.Run();