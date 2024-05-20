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
    private IToDoListXmlStorage toDoListXmlStorage;
    private IToDoRepository todoRepository;
    private ICategoryRepository categoryRepository;

    public HomeController(ILogger<HomeController> logger, ToDoListDbContext toDoListDbContext,
        IToDoListXmlStorage toDoListXmlStorage, IToDoRepository todoRepository, ICategoryRepository categoryRepository)
    {
        _logger = logger;
        this.toDoListDbContext = toDoListDbContext;
        this.toDoListXmlStorage = toDoListXmlStorage;
        this.todoRepository = todoRepository;
        this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    public IActionResult ChangeStorage(ChangeStorageRequest changeStorageRequest)
    {
        TempData["StorageName"] = changeStorageRequest.StorageName;
        return RedirectToAction("Index", changeStorageRequest);
    }


    [HttpGet]
    public async Task<IActionResult> Index(ChangeStorageRequest changeStorageRequest)
    {
        List<ToDo> todos;
        List<Category> categories;
        if (changeStorageRequest.StorageName == "DbStorage")
        {
            todos = await todoRepository.GetAll();
            categories = await categoryRepository.GetAll();
        }
        else
        {
            todos = toDoListXmlStorage.LoadAllToDosXml(
                "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
            categories = toDoListXmlStorage.LoadAllCategories(
                "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
        }

        var model = new HomePageViewModel
        {
            Categories = categories,
            ToDos = todos,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDo(AddToDoRequest addToDoRequest,
        ChangeStorageRequest changeStorageRequest)
    {
        changeStorageRequest.StorageName = TempData.Peek("StorageName")?.ToString();

        if (changeStorageRequest.StorageName == "DbStorage")
        {
            var todo = new ToDo
            {
                Id = addToDoRequest.Id,
                Task = addToDoRequest.Task,
                CategoryName = addToDoRequest.CategoryName,
                DateToPerform = addToDoRequest.DateToPerform,
            };
            await todoRepository.Add(todo);
        }
        else
        {
            XmlDocument document = new XmlDocument();

            document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

            XmlElement root = (XmlElement)document.SelectSingleNode("/database/todos")!;

            if (root != null)
            {
                XmlElement todo = document.CreateElement("todo");
                XmlElement id = document.CreateElement("id");
                XmlElement task = document.CreateElement("task");
                XmlElement isPerformed = document.CreateElement("isPerformed");
                XmlElement categoryName = document.CreateElement("categoryName");

                id.InnerText = addToDoRequest.Id.ToString();
                task.InnerText = addToDoRequest.Task;
                isPerformed.InnerText = addToDoRequest.IsPerformed.ToString();
                categoryName.InnerText = addToDoRequest.CategoryName;

                todo.AppendChild(id);
                todo.AppendChild(task);
                todo.AppendChild(isPerformed);
                todo.AppendChild(categoryName);

                if (addToDoRequest.DateToPerform != null)
                {
                    XmlElement? dateToPerform = document.CreateElement("dateToPerform");
                    dateToPerform.InnerText = addToDoRequest.DateToPerform.ToString();
                    todo.AppendChild(dateToPerform);
                }

                root.AppendChild(todo);
                toDoListXmlStorage.SaveXml(
                    "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                    document);
            }
        }

        return RedirectToAction("Index", changeStorageRequest);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryRequest addCategoryRequest,
        ChangeStorageRequest changeStorageRequest)
    {
        changeStorageRequest.StorageName = TempData["StorageName"]?.ToString();
        if (changeStorageRequest.StorageName == "DbStorage")
        {
            var category = new Category
            {
                Name = addCategoryRequest.Name,
            };
            await categoryRepository.Add(category);

        }
        else
        {
            XmlDocument document = new XmlDocument();

            document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

            XmlElement root = (XmlElement)document.SelectSingleNode("/database/categories")!;

            if (root != null)
            {
                XmlElement category = document.CreateElement("category");
                XmlElement id = document.CreateElement("id");
                XmlElement name = document.CreateElement("name");

                id.InnerText = addCategoryRequest.Id;
                name.InnerText = addCategoryRequest.Name;

                category.AppendChild(id);
                category.AppendChild(name);

                root.AppendChild(category);

                toDoListXmlStorage.SaveXml(
                    "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                    document);
            }
        }

        return RedirectToAction("Index", changeStorageRequest);
    }

    [HttpPost]
    public async Task<IActionResult> PerformToDo(PerformTodoRequest performTodoRequest,
        ChangeStorageRequest changeStorageRequest)
    {
        changeStorageRequest.StorageName = TempData["StorageName"]?.ToString();
        if (changeStorageRequest.StorageName == "DbStorage")
        {
            var perfromToDo = new ToDo
            {
                Id = performTodoRequest.Id,
                IsPerformed = true,
            };
            
           await todoRepository.ChangePerformed(perfromToDo);
        }
        else
        {
            XmlDocument document = new XmlDocument();
            document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

            string idString = performTodoRequest.Id.ToString();

            XmlNode todoNode = document.SelectSingleNode($"/database/todos/todo[id='{idString}']")!;

            if (todoNode != null)
            {
                XmlElement isPerformed = (XmlElement)todoNode.SelectSingleNode("isPerformed")!;
                if (isPerformed != null)
                {
                    isPerformed.InnerText = true.ToString();
                    document.Save("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
                }
            }
        }


        return RedirectToAction("Index", changeStorageRequest);
    }

    [HttpPost]
    public async Task<IActionResult> UnperformToDo(PerformTodoRequest performTodoRequest,
        ChangeStorageRequest changeStorageRequest)
    {
        changeStorageRequest.StorageName = TempData["StorageName"]?.ToString();
        if (changeStorageRequest.StorageName == "DbStorage")
        {
            var perfromToDo = new ToDo
            {
                Id = performTodoRequest.Id,
                IsPerformed = false,
            };

            await todoRepository.ChangePerformed(perfromToDo);
        }
        else
        {
            XmlDocument document = new XmlDocument();
            document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

            string idString = performTodoRequest.Id.ToString();

            XmlNode todoNode = document.SelectSingleNode($"/database/todos/todo[id='{idString}']")!;

            if (todoNode != null)
            {
                XmlElement isPerformed = (XmlElement)todoNode.SelectSingleNode("isPerformed")!;
                if (isPerformed != null)
                {
                    isPerformed.InnerText = false.ToString();
                    document.Save("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
                }
            }
        }

        return RedirectToAction("Index", changeStorageRequest);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteToDo(DeleteToDoRequest deleteToDoRequest,
        ChangeStorageRequest changeStorageRequest)
    {
        changeStorageRequest.StorageName = TempData["StorageName"]?.ToString();
        if (changeStorageRequest.StorageName == "DbStorage")
        {
            var todoToDelete = new ToDo
            {
                Id = deleteToDoRequest.Id,
            };

        if (todoToDelete != null)
        {
            await todoRepository.Delete(todoToDelete);
        }
        }
        else
        {
            XmlDocument document = new XmlDocument();
            document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

            string idString = deleteToDoRequest.Id.ToString();

            XmlNode? todoToDelete = document.SelectSingleNode($"/database/todos/todo[id='{idString}']");

            if (todoToDelete != null)
            {
                todoToDelete.ParentNode?.RemoveChild(todoToDelete);
                toDoListXmlStorage.SaveXml(
                    "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                    document);
            }
        }

        return RedirectToAction("Index", changeStorageRequest);
    }

}