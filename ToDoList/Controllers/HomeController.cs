using System.Diagnostics;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Models.Domain;
using System.Linq;

namespace ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ToDoListDbContext toDoListDbContext;
    private readonly XmlStorageContext xmlStorageContext;
    private IToDoListXmlRepository toDoListXmlRepository;
    private IToDoRepository todoRepository;
    private ICategoryRepository categoryRepository;

    public HomeController(ILogger<HomeController> logger, ToDoListDbContext toDoListDbContext,
        IToDoListXmlRepository toDoListXmlRepository, IToDoRepository todoRepository,
        ICategoryRepository categoryRepository,
        XmlStorageContext xmlStorageContext)
    {
        _logger = logger;
        this.toDoListDbContext = toDoListDbContext;
        this.xmlStorageContext = xmlStorageContext;
        this.toDoListXmlRepository = toDoListXmlRepository;
        this.todoRepository = todoRepository;
        this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    public IActionResult ChangeStorage(ChangeStorageRequest changeStorageRequest)
    {
        HttpContext.Session.SetString("StorageName", changeStorageRequest.StorageName);

        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        List<ToDo> todos;
        List<Category> categories;
        if (storageName == "DbStorage")
        {
            todos = await todoRepository.GetAll();
            categories = await categoryRepository.GetAll();
        }
        else
        {
            todos = toDoListXmlRepository.LoadAllToDosXml(
                xmlStorageContext.XmlStoragePath);
            categories = toDoListXmlRepository.LoadAllCategories(
                xmlStorageContext.XmlStoragePath);
        }

        var model = new HomePageViewModel
        {
            Categories = categories,
            ToDos = todos,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDo(AddToDoRequest addToDoRequest)
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        if (storageName == "DbStorage")
        {
            await todoRepository.Add(addToDoRequest);
        }
        else
        {
            toDoListXmlRepository.AddToDo(addToDoRequest);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryRequest addCategoryRequest)
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        if (storageName == "DbStorage")
        {
            await categoryRepository.Add(addCategoryRequest);
        }
        else
        {
            toDoListXmlRepository.AddCategory(addCategoryRequest);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PerformToDo(HandleTodoRequest handleTodoRequest)
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        if (storageName == "DbStorage")
        {
            await todoRepository.PerformToDo(handleTodoRequest);
        }
        else
        {
            toDoListXmlRepository.PerformToDo(handleTodoRequest);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UnperformToDo(HandleTodoRequest handleTodoRequest)
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        if (storageName == "DbStorage")
        {
            await todoRepository.UnperformToDo(handleTodoRequest);
        }
        else
        {
            toDoListXmlRepository.UnperformToDo(handleTodoRequest);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteToDo(DeleteToDoRequest deleteToDoRequest)
    {
        var storageName = HttpContext.Session.GetString("StorageName");

        if (storageName == "DbStorage")
        {
            await todoRepository.Delete(deleteToDoRequest);
        }
        else
        {
            toDoListXmlRepository.DeleteToDo(deleteToDoRequest);
        }

        return RedirectToAction("Index");
    }
}