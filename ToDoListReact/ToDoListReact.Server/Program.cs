using GraphiQl;
using GraphQL;
using GraphQL.Types;
using ToDoList.Data;
using ToDoList.Factory;
using ToDoList.Repository;
using ToDoListAPI.Mutation;
using ToDoListAPI.Query;
using ToDoListAPI.Schema;
using ToDoListAPI.Type;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ToDoListDbContext>();
builder.Services.AddSingleton<XmlStorageContext>();

builder.Services.AddSingleton<StorageChanger>();

builder.Services.AddSingleton<ToDoListDbPepository>();
builder.Services.AddSingleton<ToDoListXmlRepository>();

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
    var reactUrl = configuration.GetConnectionString("React_Url");

    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGraphiQl("/graphql");

app.UseCors();

app.UseSession();

app.UseGraphQL<ISchema>();

app.UseHttpsRedirection();

app.Run();