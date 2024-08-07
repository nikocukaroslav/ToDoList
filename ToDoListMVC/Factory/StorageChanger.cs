﻿using ToDoList.Data;
using ToDoList.Models.Domain;
using ToDoList.Repository;

namespace ToDoList.Factory;

public class StorageChanger : IToDoListFactory
{
    private readonly IServiceProvider _serviceProvider;
    private IToDoListRepository? _factory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StorageChanger(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public IToDoListRepository? GetToDoListRepository()
    {
        var storageName = _httpContextAccessor.HttpContext.Session.GetString("StorageName") ??
            _httpContextAccessor.HttpContext.Request.Headers["StorageName"].FirstOrDefault();

        if (storageName == null)
            storageName = "DbStorage";

        switch (storageName)
        {
            case "XmlStorage":
                _factory = _serviceProvider.GetRequiredService<ToDoListXmlRepository>();
                break;
            case "DbStorage":
                _factory = _serviceProvider.GetRequiredService<ToDoListDbPepository>();
                break;
            default: throw new Exception("No storage");
        }

        return _factory;
    }
}