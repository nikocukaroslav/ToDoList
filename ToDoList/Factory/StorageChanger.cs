using ToDoList.Data;

namespace ToDoList.Factory;

public class StorageChanger : IToDoListFactory
{
    private readonly IServiceProvider _serviceProvider;
    private IToDoListFactory _factory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StorageChanger(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        _factory = serviceProvider.GetRequiredService<XmlStorageFactory>();
    }

    public IToDoListRepository GetToDoListRepository()
    {
        return _factory.GetToDoListRepository();
    }

    public void ChangeStorage()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var storageName = _httpContextAccessor.HttpContext.Session.GetString("StorageName");

            switch (storageName)
            {
                case "DbStorage":
                    _factory = _serviceProvider.GetRequiredService<DbStorageFactory>();
                    break;
                case "XmlStorage":
                    _factory = _serviceProvider.GetRequiredService<XmlStorageFactory>();
                    break;
                default: throw new Exception("No storage");
            }
        }
    }
}