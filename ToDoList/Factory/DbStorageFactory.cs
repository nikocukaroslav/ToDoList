using ToDoList.Data;

namespace ToDoList.Factory;

public class DbStorageFactory :IToDoListFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DbStorageFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IToDoListRepository GetToDoListRepository()
    {
        return _serviceProvider.GetRequiredService<ToDoListDbPepository>();
    }
}