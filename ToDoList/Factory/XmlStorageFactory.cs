using ToDoList.Data;

namespace ToDoList.Factory;

public class XmlStorageFactory : IToDoListFactory
{
    private readonly IServiceProvider _serviceProvider;

    public XmlStorageFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IToDoListRepository GetToDoListRepository()
    {
        return _serviceProvider.GetRequiredService<ToDoListXmlRepository>();
    }
}